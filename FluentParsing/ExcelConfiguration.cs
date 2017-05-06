using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SpreadsheetLight;

namespace FluentParsing
{
    internal class ExcelConfiguration<T> : IExcelConfiguration<T>
    {
        private readonly Func<T> ctor;

        private readonly FieldMapping[] mappings;

        public ExcelConfiguration(Func<T> ctor, string[] fields)
        {
            this.ctor = ctor;
            mappings = fields.Select(f => new FieldMapping(f)).ToArray();
        }

        public ParseResults<T> RunFor(Stream stream)
        {
            var excel = new SLDocument(stream);

            var items = ParseWhile(excel, (e, row) => e.HasCellValue(row, 1)).ToArray();

            return new ParseResults<T>(items);
        }

        private IEnumerable<T> ParseWhile(SLDocument excel, Func<SLDocument, int, bool> predicate)
        {
            var row = 1;
            while (predicate(excel, row))
            {
                var item = ctor();

                for (var column = 0; column < mappings.Length; column++)
                {
                    var value = excel.GetCellValueAsString(row, column + 1);

                    mappings[column].Map(item, value);
                }

                yield return item;

                row++;
            }
        }

        private class FieldMapping
        {
            private readonly Action<T, object> mapping;

            public FieldMapping(string field)
            {
                var property = typeof(T).GetProperty(field);

                if (property == null)
                {
                    throw new ArgumentNullException();
                }

                // TODO remove reflection
                mapping = (item, value) =>
                {
                    property.SetValue(item, Convert.ChangeType(value, property.PropertyType));
                };
            }

            public void Map(T item, string value)
            {
                mapping(item, value);
            }
        }
    }
}
