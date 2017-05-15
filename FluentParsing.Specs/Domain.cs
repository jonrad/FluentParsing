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

        public class StringConfiguration
        {
            public StringConfigurationBuilder<T> Row<T>(string[] fields)
                where T : new()
            {
                return new StringConfigurationBuilder<T>(fields);
            }
        }

        public class StringConfigurationBuilder<T, TNext>
            where T : new()
            where TNext : new()
        {
            private Func<StringConfiguration<T>> build;

            private string[] nextFields;

            public StringConfigurationBuilder(Func<StringConfiguration<T>> build, string[] nextFields)
            {
                this.build = build;
                this.nextFields = nextFields;
            }

            public StringConfiguration<T, Result<TNext>> Build()
            {
                var child = build();
                var next = new StringConfiguration<TNext>(nextFields);

                return new StringConfiguration<T, Result<TNext>>(
                    child.Parse,
                    s => new Result<TNext>(next.Parse(s)));
            }
        }

        public class StringConfigurationBuilder<T>
            where T : new()
        {
            private readonly string[] fields;

            public StringConfigurationBuilder(string[] fields)
            {
                this.fields = fields;
            }

            public StringConfiguration<T> Build()
            {
                return new StringConfiguration<T>(fields);
            }

            public StringConfigurationBuilder<T, TNext> Row<TNext>(string[] nextFields)
                where TNext : new()
            {
                return new StringConfigurationBuilder<T, TNext>(
                    this.Build,
                    nextFields);
            }
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