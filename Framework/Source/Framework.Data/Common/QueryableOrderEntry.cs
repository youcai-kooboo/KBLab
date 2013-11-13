using System;
using System.Linq.Expressions;

namespace Framework.Data.Common
{
    public class QueryableOrder<TSource, TKey>
    {
        public QueryableOrder(Expression<Func<TSource, TKey>> expression)
        {
            this.Expression = expression;
            OrderDirection = OrderDirection.ASC;
        }

        public QueryableOrder(Expression<Func<TSource, TKey>> expression, OrderDirection orderDirection)
        {
            this.Expression = expression;
            OrderDirection = orderDirection;
        }

        public Expression<Func<TSource, TKey>> Expression { get; set; }

        public OrderDirection OrderDirection { get; set; }
    }

    public enum OrderDirection
    {
        ASC,
        DESC
    }
}