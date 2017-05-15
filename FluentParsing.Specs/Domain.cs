using System;
using System.Linq;

namespace FluentParsing.Specs
{
    // MOVE ME
    namespace Domain
    {
        public class Result<T, TNext>
        {
            public Result(T item, TNext next)
            {
                Item = item;
                Next = next;
            }

            public T Item { get; }

            public TNext Next { get; }
        }

        public class StringConfiguration
        {
            public StringConfigurationBuilder<T> Row<T>(string[] fields)
                where T : new()
            {
                return new StringConfigurationBuilder<T>(() => new T(), fields);
            }
        }

        public class StringConfigurationBuilder<T, TNext>
        {
            private readonly Func<StringConfiguration<T>> build;

            private readonly Func<TNext> createNext;

            private readonly string[] nextFields;

            public StringConfigurationBuilder(Func<StringConfiguration<T>> build, Func<TNext> createNext, string[] nextFields)
            {
                this.build = build;
                this.createNext = createNext;
                this.nextFields = nextFields;
            }

            public StringConfiguration<T, TNext> Build()
            {
                var child = build();
                var next = new StringConfiguration<TNext>(text =>
                {
                    var results = text.Read().Split(',');
                    var item = createNext();
                    var type = typeof(TNext);
                    for (var i = 0; i < nextFields.Length; i++)
                    {
                        var property = type.GetProperty(nextFields[i]);
                        property.SetValue(item, Convert.ChangeType(results[i], property.PropertyType));
                    }

                    return item;
                });

                return new StringConfiguration<T, TNext>(
                    child.Parse,
                    s => next.Parse(s));
            }

            public StringConfigurationBuilder<Result<T, TNext>, TLast> Row<TLast>(string[] lastFields)
                where TLast : new()
            {
                var child = build();
                var next = new StringConfiguration<TNext>(text =>
                {
                    var results = text.Read().Split(',');
                    var item = createNext();
                    var type = typeof(TNext);
                    for (var i = 0; i < nextFields.Length; i++)
                    {
                        var property = type.GetProperty(nextFields[i]);
                        property.SetValue(item, Convert.ChangeType(results[i], property.PropertyType));
                    }

                    return item;
                });

                return new StringConfigurationBuilder<Result<T, TNext>, TLast>(
                    () => new StringConfiguration<Result<T, TNext>>(s => new Result<T, TNext>(child.Parse(s), next.Parse(s))),
                    () => new TLast(), 
                    lastFields);


                // FIX
                return null;
            }
        }

        public class StringConfigurationBuilder<T>
        {
            private readonly Func<T> createItem;

            private readonly string[] fields;

            public StringConfigurationBuilder(Func<T> createItem, string[] fields)
            {
                this.createItem = createItem;
                this.fields = fields;
            }

            public StringConfiguration<T> Build()
            {
                return new StringConfiguration<T>(text =>
                {
                    var results = text.Read().Split(',');
                    var item = createItem();
                    var type = typeof(T);
                    for (var i = 0; i < fields.Length; i++)
                    {
                        var property = type.GetProperty(fields[i]);
                        property.SetValue(item, Convert.ChangeType(results[i], property.PropertyType));
                    }

                    return item;
                });
            }

            public StringConfigurationBuilder<T, TNext> Row<TNext>(string[] nextFields)
                where TNext : new()
            {
                return new StringConfigurationBuilder<T, TNext>(
                    Build,
                    () => new TNext(), 
                    nextFields);
            }
        }

        //TODO MOVE
    public class Context
    {
        public Context(string text)
        {
             Lines = text.Split(new [] { Environment.NewLine }, StringSplitOptions.None);
        }

        public string[] Lines { get; set; }

        public int CurrentLine = 0;

        public string Read()
        {
            return Lines[CurrentLine++];
        }
    }

        public class StringConfiguration<T, TNext>
        {
            private readonly Func<Context, T> createT;

            private readonly Func<Context, TNext> createNext;

            public StringConfiguration(Func<Context, T> createT, Func<Context, TNext> createNext)
            {
                this.createT = createT;
                this.createNext = createNext;
            }

            public Result<T, TNext> Parse(Context text)
            {
                return new Result<T, TNext>(createT(text), createNext(text));
            }
        }

        public class StringConfiguration<T>
        {
            private readonly Func<Context, T> parse;

            public StringConfiguration(Func<Context, T> parse)
            {
                this.parse = parse;
            }

            public virtual T Parse(Context text)
            {
                return parse(text);
                /*
                var results = text.Split(',');
                var item = newItem();
                var type = typeof(T);
                for (var i = 0; i < fields.Length; i++)
                {
                    var property = type.GetProperty(fields[i]);
                    property.SetValue(item, Convert.ChangeType(results[i], property.PropertyType));
                }

                return item;
                */
            }
        }
    }
}