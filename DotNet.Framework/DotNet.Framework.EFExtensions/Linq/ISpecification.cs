using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DotNet.Framework.EFExtensions.Linq
{

    public interface ISpecification<TEntity> where TEntity : class
    {
        ISpecification<TEntity> And(ISpecification<TEntity> other);
        ISpecification<TEntity> AndNot(ISpecification<TEntity> other);
        Expression<Func<TEntity, bool>> GetExpression();
        bool IsSatisfiedBy(TEntity entity);
        ISpecification<TEntity> Not();
        ISpecification<TEntity> Or(ISpecification<TEntity> other);
    }
}
