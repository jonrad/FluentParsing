using System;

namespace FluentParsing
{
    internal class FluentSheet : IFluentSheet
    {
        private string sheetName;

        public FluentSheet(string sheetName)
        {
            this.sheetName = sheetName;
        }

        public IFluentRow<T> ParseRows<T>()
            where T : new()
        {
            return ParseRows(() => new T());
        }

        public IFluentRow<T> ParseRows<T>(Func<T> ctor)
        {
            return new FluentRow<T>(ctor);
        }
    }
}