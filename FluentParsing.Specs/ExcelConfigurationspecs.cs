using System.Linq;
using Machine.Specifications;

namespace FluentParsing.Specs
{
    class ExcelConfigurationspecs
    {
        static ExcelConfiguration<Sample> configuration;

        static ParseResults<Sample> results;

        class in_general
        {
            Establish context = () =>
                configuration = new ExcelConfiguration<Sample>();

            Because of = () =>
                results = configuration.RunFor("./sample.xls");

            It parsed_successfully = () =>
                results.Successful.ShouldEqual(true);

            It returned_0_count = () =>
                results.Count.ShouldEqual(0);

            It returned_no_results = () =>
                results.Items.Count().ShouldEqual(0);
        }

        class Sample
        {
            public string Name { get; set; }

            public string Age { get; set; }
        }
    }
}
