namespace shipman.Server.Infrastructure.Extensions
{
    using System.Linq.Expressions;

    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderByProperty<T>(
            this IQueryable<T> source,
            string propertyName,
            bool ascending)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.PropertyOrField(parameter, propertyName);

            var lambda = Expression.Lambda(property, parameter);

            string methodName = ascending ? "OrderBy" : "OrderByDescending";

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda })!;
        }
    }
}
