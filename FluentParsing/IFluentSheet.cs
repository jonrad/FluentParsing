namespace FluentParsing
{
    public interface IFluentSheet
    {
        IFluentRow<T> ParseRows<T>();
    }
}