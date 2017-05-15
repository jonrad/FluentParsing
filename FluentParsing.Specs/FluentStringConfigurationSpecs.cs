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