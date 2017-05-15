using System;

namespace FluentParsing.Specs
{
    public static class StringFixtures
    {
        public static Func<Domain.Context, T> Create<T>(params string[] fields) where T : new()
        {
            return context =>
            {
                var results = context.Read().Split(',');
                var item = new T();
                var type = typeof(T);
                for (var i = 0; i < fields.Length; i++)
                {
                    var property = type.GetProperty(fields[i]);
                    property.SetValue(item, Convert.ChangeType(results[i], property.PropertyType));
                }

                return item;
            }; 
        }
    }
}