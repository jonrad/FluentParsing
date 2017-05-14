using FluentParsing.Specs.Domain;

namespace FluentParsing.Specs
{
    class FluentStringConfigurationSpecs
    {
        class simple
        {
            private static Person result;

            private static StringConfiguration<Person> configuration;
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