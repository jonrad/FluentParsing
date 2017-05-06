using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentParsing
{
    internal class FluentRow<T> : IFluentRow<T>
    {
        // TODO make this immutable stop returning 'this'
        private readonly List<string> fields = new List<string>();

        private readonly Func<T> ctor;

        public FluentRow(Func<T> ctor)
        {
            this.ctor = ctor;
        }

        public IFluentRow<T> WithField(string setterField)
        {
            fields.Add(setterField);
            return this;
        }

        public IFluentRow<T> WithField<TField>(Expression<Func<T, TField>> field)
        {
            throw new NotImplementedException();
        }

        public IExcelConfiguration<T> Build()
        {
            return new ExcelConfiguration<T>(ctor, fields.ToArray());
        }
    }
}