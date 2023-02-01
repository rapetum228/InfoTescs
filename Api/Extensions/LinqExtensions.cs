using System.Linq.Expressions;

namespace InfoTecs.Api.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> whereClause)
        {
            return condition ? query.Where(whereClause) : query;
        }
    }
}
