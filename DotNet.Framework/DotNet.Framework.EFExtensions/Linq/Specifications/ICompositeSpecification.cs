using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Framework.EFExtensions.Linq.Specifications
{

    internal interface ICompositeSpecification<TEntity> : ISpecification<TEntity> where TEntity: class
    {
        ISpecification<TEntity> Left { get; }
        ISpecification<TEntity> Right { get; }
    }
}
