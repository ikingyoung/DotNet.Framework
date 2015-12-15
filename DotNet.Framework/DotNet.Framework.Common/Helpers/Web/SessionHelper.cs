using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DotNet.Framework.Common.Helpers.Web
{
    /// <summary>
    /// Session 操作类
    /// 1、GetSession(string name)根据session名获取session对象
    /// 2、SetSession(string name, object val)设置session
    /// </summary>
    public class SessionHelper
    {
        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetSession(string name)
        {
            return HttpContext.Current.Session[name];
        }
        public static T GetSession<T>(string name)
        {
            var value = HttpContext.Current.Session[name];
            if(value==null)
            {
                return default(T);
            }
            else
            {
                return (T)(object)value;
            }
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="value">session 值</param>
        /// <param name="timeOut">过期分钟数（默认20）</param>
        public static void SetSession(string name, object value,int timeOut=20)
        {
            HttpContext.Current.Session[name] = value;
            HttpContext.Current.Session.Timeout = timeOut;
        }

        /// <summary>
        /// 清空所有的Session
        /// </summary>
        /// <returns></returns>
        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        /// <summary>
        /// 删除一个指定的ession
        /// </summary>
        /// <param name="name">Session名称</param>
        /// <returns></returns>
        public static void RemoveSession(string name)
        {
            HttpContext.Current.Session.Remove(name);
        }

        /// <summary>
        /// 删除所有的ession
        /// </summary>
        /// <returns></returns>
        public static void RemoveAllSession()
        {
            HttpContext.Current.Session.RemoveAll();
        }
    }
}
