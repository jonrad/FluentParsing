using System;
using System.Linq.Expressions;

namespace FluentParsing
{
    internal class FluentRow<T> : IFluentRow<T>
    {
        public IFluentRow<T> WithField(Action<T, object> setValue)
        {
            return this;
        }

        public IFluentRow<T> WithField<TField>(Expression<Func<T, TField>> field)
        {
            return this;
        }
    }
}