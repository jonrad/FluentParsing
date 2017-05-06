namespace FluentParsing
{
    internal class FluentSheet : IFluentSheet
    {
        private string sheetName;

        public FluentSheet(string sheetName)
        {
            this.sheetName = sheetName;
        }

        public IFluentRow<T> ParseRows<T>()
        {
            return new FluentRow<T>();
        }
    }
}