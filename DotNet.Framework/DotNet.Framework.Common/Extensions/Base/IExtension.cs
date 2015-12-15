using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNet.Framework.Common.Extensions
{
    public interface IExtension<V>
    {
        V Self { get; }

        //V GetValue();
    }
}
