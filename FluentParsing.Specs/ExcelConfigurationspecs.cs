using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Machine.Specifications;

namespace FluentParsing.Specs
{
    class ExcelConfigurationSpecs
    {
        static ExcelConfiguration<Sample> configuration;

        static ParseResults<Sample> results;

        class when_parsing
        {
            static Stream stream;

            Establish context = () =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                stream = assembly.GetManifestResourceStream("FluentParsing.Specs.Excel.Names.xlsx");

                configuration = new ExcelConfiguration<Sample>(
                    () => new Sample(), new[]
                    {
                        nameof(Sample.Name),
                        nameof(Sample.Age)
                    });
            };

            Cleanup cleanup = () =>
                stream?.Dispose();

            Because of = () =>
                results = configuration.RunFor(stream);

            It parsed_successfully = () =>
                results.Successful.ShouldEqual(true);

            It returned_2_count = () =>
                results.Count.ShouldEqual(2);

            It returned_proper_first_result = () =>
            {
                var item1 = results.Items.First();

                item1.Name.ShouldEqual("Jon");
                item1.Age.ShouldEqual(20);
            };

            It returned_proper_second_result = () =>
            {
                var item2 = results.Items.Skip(1).First();

                item2.Name.ShouldEqual("Mike");
                item2.Age.ShouldEqual(25);
            };
        }

        class Sample
        {
            public string Name { get; set; }

            public int Age { get; set; }
        }
    }
}
