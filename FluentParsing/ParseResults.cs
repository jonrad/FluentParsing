using System.Collections.Generic;

namespace FluentParsing
{
    public class ParseResults<T>
    {
        public bool Successful { get; private set; }

        public int Count { get; private set; }

        public IEnumerable<T> Items { get; private set; }
    }
}