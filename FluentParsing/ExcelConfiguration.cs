using System;

namespace FluentParsing
{
    internal class ExcelConfiguration<T>
    {
        public ParseResults<T> RunFor(string path)
        {
            return new ParseResults<T>();
        }
    }
}
