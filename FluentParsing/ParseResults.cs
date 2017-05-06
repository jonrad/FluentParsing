using System.Collections.Generic;

namespace FluentParsing
{
    public class ParseResults<T>
    {
        private readonly T[] items;

        public ParseResults(T[] items)
        {
            this.items = items;
        }

        public bool Successful => true;

        public int Count => items.Length;

        public IEnumerable<T> Items => items;
    }
}