using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Framework.Common.Linq
{
    [Serializable]
    public class PagingCondition
    {
        public PagingCondition()
        {
            this.RecordCount = 0;
            this.PageSize = 20;
            this.CurrentPage = 1;
        }
        public virtual int CurrentPage { get; set; }
        public bool IsGetRecordCount { get; set; }
        public virtual int PageCount
        {
            get
            {
                if (this.PageSize <= 0) return 0;
                return ((this.RecordCount - 1) / this.PageSize) + 1;
            }
        }
        public virtual int PageSize { get; set; }
        public virtual int RecordCount { get; set; }
        public override string ToString()
        {
            var format = "<PagingCondition PageSize=\"{0}\" CurrentPage=\"{1}\" IsGetRecordCount=\"{2}\" RecordCount=\"{3}\" PageCount=\"{4}\" />";

            return string.Format(format, this.PageSize, this.CurrentPage, this.IsGetRecordCount, this.RecordCount, this.PageCount);
        }

    }
}
