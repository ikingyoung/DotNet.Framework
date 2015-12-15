using DotNet.Framework.Common.Linq.Specifications;
using System;
using System.Linq.Expressions;

namespace DotNet.Framework.Common.Linq
{
    public abstract class Specification<TEntity>:ISpecification<TEntity> where TEntity:class
    {

        public virtual ISpecification<TEntity> And(ISpecification<TEntity> other)
        {
            return other == null ? this : new AndSpecification<TEntity>(this, other);
        }
        public virtual ISpecification<TEntity> AndNot(ISpecification<TEntity> other)
        {
            return other == null ? this : new AndNotSpecification<TEntity>(this, other);
        }
        public abstract Expression<Func<TEntity, bool>> GetExpression();
        public virtual bool IsSatisfiedBy(TEntity entity)
        {
            return this.GetExpression().Compile()(entity);
        }
        public virtual ISpecification<TEntity> Not()
        {
            return new NotSpecification<TEntity>(this);
        }
        public virtual ISpecification<TEntity> Or(ISpecification<TEntity> other)
        {
            return other == null ? this : new OrSpecification<TEntity>(this, other);
        }
        public static ISpecification<TEntity> Create(Expression<Func<TEntity,bool>> expression)
        {
            return new ExpressionSpecification<TEntity>(expression);
        }

    }
}
