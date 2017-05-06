using System;

namespace FluentParsing
{
    public interface IFluentRow<T>
    {
        IFluentRow<T> WithField(string setterField);

        IFluentRow<T> WithField<TField>(System.Linq.Expressions.Expression<Func<T, TField>> field);

        IExcelConfiguration<T> Build();
    }
}