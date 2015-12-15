using System;
using System.Linq.Expressions;

namespace DotNet.Framework.Common.Linq.Specifications
{
    internal class AndNotSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : class
    {
        public AndNotSpecification(ISpecification<TEntity> left,ISpecification<TEntity> right):base(left,right)
        {
           
        }
        public override Expression<Func<TEntity, bool>> GetExpression()
        {
            var parameterExpression = Expression.Parameter(typeof(TEntity));
            var parameterReplacer = new ParameterReplacer(parameterExpression);
            var expressionReplaceLeft = parameterReplacer.Replace(base.Left.GetExpression().Body);
            var expressionReplaceRight = parameterReplacer.Replace(Expression.Not(base.Right.GetExpression().Body));
            var expressionResult = Expression.Lambda<Func<TEntity, bool>>(Expression.Add(expressionReplaceLeft, expressionReplaceRight), new ParameterExpression[] { parameterExpression });

            return expressionResult;
        }
    }
}
