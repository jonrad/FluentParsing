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
            where T : new() // TODO shitty hack
        {
            return new FluentRow<T>();
        }
    }
}