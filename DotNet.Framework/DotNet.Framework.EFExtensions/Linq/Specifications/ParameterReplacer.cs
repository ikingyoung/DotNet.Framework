using System;
using System.Linq.Expressions;

namespace DotNet.Framework.EFExtensions.Linq.Specifications
{
    internal class ParameterReplacer:ExpressionVisitor
    {
        public ParameterReplacer(ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }
        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return this.ParameterExpression;
        }
        public ParameterExpression ParameterExpression { get; set; }
    }
}
