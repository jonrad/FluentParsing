using System;
using System.Linq;
using Machine.Specifications;

namespace FluentParsing.Specs
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
            var split = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
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

    class StringConfigurationSpecs
    {
        class simple
        {
            private static Person result;

            private static StringConfiguration<Person> subject;

            private Establish context = () =>
                subject = new StringConfiguration<Person>(new[] { nameof(Person.Name), nameof(Person.Age) });

            Because of = () =>
                result = subject.Parse("Jon,25");

            It returned_expected_name = () =>
                result.Name.ShouldEqual("Jon");

            It returned_expected_age = () =>
                result.Age.ShouldEqual(25);
        }

        public static Func<string, T> Create<T>(string[] fields) where T : new()
        {
            return new StringConfiguration<T>(fields).Parse;
        }

        public static Func<string, Result<T>> CreateResult<T>(string[] fields) where T : new()
        {
            var config = new StringConfiguration<T>(fields);
            return s => new Result<T>(config.Parse(s));
        }

        public static Func<string, Result<T, Result<TNext>>> Create<T, TNext>(string[] fields, string[] fields2) 
            where T : new()
            where TNext : new()
        {
            return new StringConfiguration<T, Result<TNext>>(Create<T>(fields), CreateResult<TNext>(fields2)).Parse;
        }

        class more_complicated
        {
            static Result<Person, Result<Dog>> result;

            static StringConfiguration<Person, Result<Dog>> subject;

            Establish context = () =>
                subject = new StringConfiguration<Person, Result<Dog>>(
                    Create<Person>(new[] { nameof(Person.Name), nameof(Person.Age) }),
                    CreateResult<Dog>(new[] { nameof(Dog.Name), nameof(Dog.Breed) }));

            Because of = () =>
                result = subject.Parse($"Jon,25{Environment.NewLine}Janie,Shiba");

            It returned_expected_name = () =>
                result.Item.Name.ShouldEqual("Jon");

            It returned_expected_age = () =>
                result.Item.Age.ShouldEqual(25);

            It returned_expected_dog_name = () =>
                result.Next.Item.Name.ShouldEqual("Janie");

            It returned_expected_dog_breed = () =>
                result.Next.Item.Breed.ShouldEqual("Shiba");
        }

        class most_complicated
        {
            static Result<Person, Result<Dog, Result<Horse>>> result;

            static StringConfiguration<Person, Result<Dog, Result<Horse>>> subject;

            Establish context = () =>
                subject = new StringConfiguration<Person, Result<Dog, Result<Horse>>>(
                    Create<Person>(new[] { nameof(Person.Name), nameof(Person.Age) }),
                    Create<Dog, Horse>(
                        new[] { nameof(Dog.Name), nameof(Dog.Breed) },
                        new[] { nameof(Horse.Name), nameof(Horse.Speed) }));

            Because of = () =>
                result = subject.Parse($"Jon,25{Environment.NewLine}Janie,Shiba{Environment.NewLine}Wilber,15");

            It returned_expected_name = () =>
                result.Item.Name.ShouldEqual("Jon");

            It returned_expected_age = () =>
                result.Item.Age.ShouldEqual(25);

            It returned_expected_dog_name = () =>
                result.Next.Item.Name.ShouldEqual("Janie");

            It returned_expected_dog_breed = () =>
                result.Next.Item.Breed.ShouldEqual("Shiba");

            It returned_expected_horse_name = () =>
                result.Next.Next.Item.Name.ShouldEqual("Wilber");

            It returned_expected_horse_speed = () =>
                result.Next.Next.Item.Speed.ShouldEqual(15);
        }

        public class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        public class Dog
        {
            public string Breed { get; set; }

            public string Name { get; set; }
        }

        public class Horse
        {
            public string Name { get; set; }

            public int Speed { get; set; }
        }
    }
}