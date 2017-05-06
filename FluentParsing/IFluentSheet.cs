using System;

namespace FluentParsing
{
    public interface IFluentSheet
    {
        IFluentRow<T> ParseRows<T>()
            where T : new();

        IFluentRow<T> ParseRows<T>(Func<T> ctor);
    }
}