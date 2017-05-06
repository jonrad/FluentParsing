using System.IO;

namespace FluentParsing
{
    public interface IExcelConfiguration<T>
    {
        ParseResults<T> RunFor(Stream stream);
    }
}