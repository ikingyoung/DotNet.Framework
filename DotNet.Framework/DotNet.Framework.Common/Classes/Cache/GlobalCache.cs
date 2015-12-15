using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Classes.Cache
{
    public class GlobalCache
    {
        private static ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();
        public static void set(string key, object value)
        {
            if (!_cache.TryAdd(key, value))
            {
                //throw new Exception(string.Format("the cache key '{0}' is repeat.",key));
            }
        }
        public static object get(string key)
        {
            object value;
            if (_cache.TryGetValue(key, out value))
            {
                value = null;
            }
            return value;

        }
        public static T get<T>(string key)
        {
            T result = default(T);
            object value;
            if (!_cache.TryGetValue(key, out value))
            {
                if (value is T) result = (T)(object)value;
            }
            return result;

        }
    }
}
