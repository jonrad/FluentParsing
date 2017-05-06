namespace FluentParsing
{
    public interface IFluentSheet
    {
        IFluentRow<T> ParseRows<T>()
            where T : new(); // TODO shitty hack
    }
}