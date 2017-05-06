namespace FluentParsing
{
    internal class FluentExcel : IFluentExcel
    {
        public IFluentSheet ForSheet(string sheetName)
        {
            return new FluentSheet(sheetName);
        }
    }
}