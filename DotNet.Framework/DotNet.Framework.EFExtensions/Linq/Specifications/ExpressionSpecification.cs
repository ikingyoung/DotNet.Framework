using System;
using System.Linq.Expressions;

namespace DotNet.Framework.EFExtensions.Linq.Specifications
{
    internal class ExpressionSpecification<TEntity> : Specification<TEntity> where TEntity : class
    {
        private Expression<Func<TEntity, bool>> _expression;
        public ExpressionSpecification(Expression<Func<TEntity, bool>> expression)
        {
            this._expression= expression;
        }

        public override Expression<Func<TEntity, bool>> GetExpression()
        {
            return this._expression;
        }
    }
}
