using System.Linq.Expressions;

namespace Autoplace.Common.Extensions
{
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                            Expression<Func<T, bool>> expr2)
            => Expression.Lambda<Func<T, bool>>
            (Expression.OrElse(expr1.Body, Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>())), expr1.Parameters);

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                             Expression<Func<T, bool>> expr2)
            => Expression.Lambda<Func<T, bool>>
            (Expression.AndAlso(expr1.Body, Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>())), expr1.Parameters);
    }
}
