using System;
using System.Linq;

namespace FluentParsing.Specs
{
    // MOVE ME
    namespace Domain
    {
        public class Result<T>
        {
            public Result(T item)
            {
                Item = item;
            }

            public T Item { get; }
        }

        public class Result<T, TNext>
            : Result<T>
        {
            public Result(T item, TNext next)
                : base(item)
            {
                Next = next;
            }

            public TNext Next { get; }
        }

        public class StringConfiguration<T, TNext>
        {
            private readonly Func<string, T> createT;

            private readonly Func<string, TNext> createNext;

            public StringConfiguration(Func<string, T> createT, Func<string, TNext> createNext)
            {
                this.createT = createT;
                this.createNext = createNext;
            }

            public Result<T, TNext> Parse(string text)
            {
                var split = text.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
                var nextLines = string.Join(Environment.NewLine, split.Skip(1));

                return new Result<T, TNext>(createT(split[0]), createNext(nextLines));
            }
        }

        public class StringConfiguration<T>
            where T : new()
        {
            private readonly string[] fields;

            public StringConfiguration(string[] fields)
            {
                this.fields = fields;
            }

            public virtual T Parse(string text)
            {
                var results = text.Split(',');
                var item = new T();
                var type = typeof(T);
                for (var i = 0; i < fields.Length; i++)
                {
                    var property = type.GetProperty(fields[i]);
                    property.SetValue(item, Convert.ChangeType(results[i], property.PropertyType));
                }

                return item;
            }
        }
    }
}