using System.Collections.Generic;

namespace FluentParsing
{
    public class ParseResults<T>
    {
        public ParseResults()
        {
            Items = new T[0];
            Count = 0;
            Successful = true;
        }

        public bool Successful { get; }

        public int Count { get; }

        public IEnumerable<T> Items { get; }
    }
}