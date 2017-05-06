using System;

namespace FluentParsing
{
    public interface IFluentRow<T>
    {
        IFluentRow<T> WithField(Action<T, object> setValue);

        IFluentRow<T> WithField<TField>(System.Linq.Expressions.Expression<Func<T, TField>> field);
    }
}