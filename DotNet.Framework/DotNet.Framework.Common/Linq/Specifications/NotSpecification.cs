using System;
using System.Linq.Expressions;

namespace DotNet.Framework.Common.Linq.Specifications
{
    internal class NotSpecification<TEntity> : Specification<TEntity> where TEntity : class
    {
        private ISpecification<TEntity> _specification;
        public NotSpecification(ISpecification<TEntity> specification)
        {
            this._specification = specification;
        }
        public override System.Linq.Expressions.Expression<Func<TEntity, bool>> GetExpression()
        {
            return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(this._specification.GetExpression().Body),this._specification.GetExpression().Parameters);
        }
    }
}
