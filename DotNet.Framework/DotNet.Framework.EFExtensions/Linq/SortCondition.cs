using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DotNet.Framework.EFExtensions.Linq
{
    public class SortCondition
    {
        /// <summary>
        /// 构建排序条件
        /// </summary>
        /// <param name="orderKey">排序字段</param>
        /// <param name="isAscending">是否顺序排序，默认为true</param>
        public SortCondition(string orderKey,bool isAscending=true)
        {
            this.OrderKey = orderKey;
            this.IsAscending = isAscending;
        }

        /// <summary>
        /// 是否顺序排序
        /// <remarks>
        /// true : 顺序排，false ： 倒序排
        /// </remarks>
        /// </summary>
        public bool IsAscending { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderKey { get; set; }
    }
    public class SortCondition<TEntity> where TEntity : class
    {      
        public SortCondition(Expression<Func<TEntity,object>> expression,bool isAscending=true)
        {
            this.Expression = expression;
            this.IsAscending = isAscending;
        }
        public Expression<Func<TEntity,object>> Expression { get; set; }
        public bool IsAscending { get; set; }
    }
    
}
