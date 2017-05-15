using System;
using System.Linq;
using FluentParsing.Specs.Domain;
using Machine.Specifications;

namespace FluentParsing.Specs
{
    class StringConfigurationSpecs
    {
        static Func<Context, T> Create<T>(string[] fields) where T : new()
        {
            return text =>
            {
                var results = text.Lines[text.CurrentLine++].Split(',');
                var item = new T();
                var type = typeof(T);
                for (var i = 0; i < fields.Length; i++)
                {
                    var property = type.GetProperty(fields[i]);
                    property.SetValue(item, Convert.ChangeType(results[i], property.PropertyType));
                }

                return item;
            }; 
        }

        class simple
        {
            static Person result;

            static StringConfiguration<Person> configuration;

            private Establish context = () =>
                configuration = new StringConfiguration<Person>(
                    text =>
                    {
                        var fields = new[] {nameof(Person.Name), nameof(Person.Age)};
                        var results = text.Read().Split(',');
                        var item = new Person();
                        var type = typeof(Person);
                        for (var i = 0; i < fields.Length; i++)
                        {
                            var property = type.GetProperty(fields[i]);
                            property.SetValue(item, Convert.ChangeType(results[i], property.PropertyType));
                        }

                        return item;
                    }); 

            Because of = () =>
                result = configuration.Parse(new Context("Jon,25"));

            It returned_expected_name = () =>
                result.Name.ShouldEqual("Jon");

            It returned_expected_age = () =>
                result.Age.ShouldEqual(25);
        }

        class more_complicated_config
        {
            static Result<Person, Dog> result;

            static StringConfiguration<Person, Dog> configuration;

            Establish context = () =>
                configuration = new StringConfiguration<Person, Dog>(
                    Create<Person>(new[] { nameof(Person.Name), nameof(Person.Age) }),
                    Create<Dog>(new[] { nameof(Dog.Name), nameof(Dog.Breed) }));

            Because of = () =>
                result = configuration.Parse(new Context($"Jon,25{Environment.NewLine}Janie,Shiba"));

            It returned_expected_name = () =>
                result.Item.Name.ShouldEqual("Jon");

            It returned_expected_age = () =>
                result.Item.Age.ShouldEqual(25);

            It returned_expected_dog_name = () =>
                result.Next.Name.ShouldEqual("Janie");

            It returned_expected_dog_breed = () =>
                result.Next.Breed.ShouldEqual("Shiba");
        }

        class most_complicated_config
        {
            static Result<Result<Person, Dog>, Horse> result;

            static StringConfiguration<Result<Person, Dog>, Horse> configuration;

            private Establish context = () =>
                configuration = new StringConfiguration<Result<Person, Dog>, Horse>(
                    s =>
                    {
                        var person = Create<Person>(new[] {nameof(Person.Name), nameof(Person.Age)});
                        var dog = Create<Dog>(new[] {nameof(Dog.Name), nameof(Dog.Breed)});

                        return new Result<Person, Dog>(person(s), dog(s));
                    },
                    Create<Horse>(new[] {nameof(Horse.Name), nameof(Horse.Speed) }));

        Because of = () =>
                result = configuration.Parse(new Context($"Jon,25{Environment.NewLine}Janie,Shiba{Environment.NewLine}Wilber,15"));

            It returned_expected_name = () =>
                result.Item.Item.Name.ShouldEqual("Jon");

            It returned_expected_age = () =>
                result.Item.Item.Age.ShouldEqual(25);

            It returned_expected_dog_name = () =>
                result.Item.Next.Name.ShouldEqual("Janie");

            It returned_expected_dog_breed = () =>
                result.Item.Next.Breed.ShouldEqual("Shiba");

            It returned_expected_horse_name = () =>
                result.Next.Name.ShouldEqual("Wilber");

            It returned_expected_horse_speed = () =>
                result.Next.Speed.ShouldEqual(15);
        }

        class Person
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        class Dog
        {
            public string Breed { get; set; }

            public string Name { get; set; }
        }

        class Horse
        {
            public string Name { get; set; }

            public int Speed { get; set; }
        }
    }
}