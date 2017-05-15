using System;
using FluentParsing.Specs.Domain;
using Machine.Specifications;

namespace FluentParsing.Specs
{
    class FluentStringConfigurationSpecs
    {
        class simple
        {
            static Person result;

            static StringConfiguration<Person> configuration;

            Establish context = () =>
                configuration = new StringConfiguration()
                    .Row<Person>(new[] { nameof(Person.Name), nameof(Person.Age) })
                    .Build();

            Because of = () =>
                result = configuration.Parse("Jon,25");

            It returned_expected_name = () =>
                result.Name.ShouldEqual("Jon");

            It returned_expected_age = () =>
                result.Age.ShouldEqual(25);
        }

        class more_complicated
        {
            static Result<Person, Result<Dog>> result;

            static StringConfiguration<Person, Result<Dog>> configuration;

            Establish context = () =>
                configuration = new StringConfiguration()
                    .Row<Person>(new[] { nameof(Person.Name), nameof(Person.Age) })
                    .Row<Dog>(new[] { nameof(Dog.Name), nameof(Dog.Breed) })
                    .Build();

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

            /*Establish context = () =>
                configuration = new StringConfiguration()
                    .Row<Person>(new[] { nameof(Person.Name), nameof(Person.Age) })
                    .Row<Dog>(new[] { nameof(Dog.Name), nameof(Dog.Breed) })
                    .Row<Horse>(new[] { nameof(Horse.Name), nameof(Horse.Speed) })
                    .Build();*/

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