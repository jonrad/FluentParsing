using System;
using FluentParsing.Specs.Domain;
using Machine.Specifications;

namespace FluentParsing.Specs
{
    class StringConfigurationSpecs
    {
        class simple
        {
            static Person result;

            static StringConfiguration<Person> configuration;

            Establish context = () =>
                configuration = new StringConfiguration<Person>(new[] { nameof(Person.Name), nameof(Person.Age) });

            Because of = () =>
                result = configuration.Parse("Jon,25");

            It returned_expected_name = () =>
                result.Name.ShouldEqual("Jon");

            It returned_expected_age = () =>
                result.Age.ShouldEqual(25);
        }

        static Func<string, T> Create<T>(string[] fields) where T : new()
        {
            return new StringConfiguration<T>(fields).Parse;
        }

        static Func<string, Result<T>> CreateResult<T>(string[] fields) where T : new()
        {
            var config = new StringConfiguration<T>(fields);
            return s => new Result<T>(config.Parse(s));
        }

        static Func<string, Result<T, Result<TNext>>> Create<T, TNext>(string[] fields, string[] fields2) 
            where T : new()
            where TNext : new()
        {
            return new StringConfiguration<T, Result<TNext>>(Create<T>(fields), CreateResult<TNext>(fields2)).Parse;
        }

        class more_complicated
        {
            static Result<Person, Result<Dog>> result;

            static StringConfiguration<Person, Result<Dog>> configuration;

            Establish context = () =>
                configuration = new StringConfiguration<Person, Result<Dog>>(
                    Create<Person>(new[] { nameof(Person.Name), nameof(Person.Age) }),
                    CreateResult<Dog>(new[] { nameof(Dog.Name), nameof(Dog.Breed) }));

            Because of = () =>
                result = configuration.Parse($"Jon,25{Environment.NewLine}Janie,Shiba");

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

            static StringConfiguration<Person, Result<Dog, Result<Horse>>> configuration;

            Establish context = () =>
                configuration = new StringConfiguration<Person, Result<Dog, Result<Horse>>>(
                    Create<Person>(new[] { nameof(Person.Name), nameof(Person.Age) }),
                    Create<Dog, Horse>(
                        new[] { nameof(Dog.Name), nameof(Dog.Breed) },
                        new[] { nameof(Horse.Name), nameof(Horse.Speed) }));

            Because of = () =>
                result = configuration.Parse($"Jon,25{Environment.NewLine}Janie,Shiba{Environment.NewLine}Wilber,15");

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