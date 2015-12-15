using System;
using System.Linq.Expressions;

namespace DotNet.Framework.EFExtensions.Linq.Specifications
{
    internal class AndSpecification<TEntity>: CompositeSpecification<TEntity> where TEntity:class
    {
        public AndSpecification(ISpecification<TEntity> left,ISpecification<TEntity> right)
            :base(left,right)
        {

        }
        public override System.Linq.Expressions.Expression<Func<TEntity, bool>> GetExpression()
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity));
            var parameterReplacer = new ParameterReplacer(parameterExpression);
            var expressionReplaceLeft = parameterReplacer.Replace(base.Left.GetExpression().Body);
            var expressionReplaceRight = parameterReplacer.Replace(base.Right.GetExpression().Body);
            var expressionResult = Expression.Lambda<Func<TEntity, bool>>(Expression.Add(expressionReplaceLeft, expressionReplaceRight), new ParameterExpression[] { parameterExpression });

            return expressionResult;
        }
    }
}
