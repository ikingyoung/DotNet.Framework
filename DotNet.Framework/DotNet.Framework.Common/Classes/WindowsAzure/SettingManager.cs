using DotNet.Framework.Common.Classes.Cache;
using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Classes.WindowsAzure
{ /// <summary>
  /// 非账号的配置操作
  /// </summary>
    public class SettingManager
    {
        private static string Get(string keyPrefix, string key, bool isAzureSetting, bool isAppSetting = true)
        {
            var tmpKey = string.Empty;// string.Format("{0}.{1}", keyPrefix, key);


            var value = string.Empty;
            if (isAzureSetting)
            {
                tmpKey = string.Format("Azure.{0}.{1}", keyPrefix, key);
                value = CloudConfigurationManager.GetSetting(key);

                GlobalCache.set(tmpKey, value);
                return value;
            }
            else
            {
                if (isAppSetting)
                {
                    tmpKey = string.Format("AppSetting.{0}.{1}", keyPrefix, key);
                    value = ConfigurationManager.AppSettings[key];
                    GlobalCache.set(tmpKey, value);
                    return value;
                }
                else
                {
                    tmpKey = string.Format("ConnectString.{0}.{1}", keyPrefix, key);
                    var setting = ConfigurationManager.ConnectionStrings[key];
                    if (null != setting)
                    {
                        value = setting.ConnectionString;
                        GlobalCache.set(tmpKey, value);
                        return value;
                    }
                }
            }
            return null;
        }
        public static string GetAzureSetting(string keyPrefix, string key)
        {
            if (string.IsNullOrEmpty(keyPrefix)) return null;
            if (string.IsNullOrEmpty(key)) return null;
            return Get(keyPrefix, key, true);
        }
        private static string GetConnectString(string keyPrefix, string key)
        {
            return Get(keyPrefix, key, false, false);
        }
        private static string GetAppSetting(string keyPrefix, string key)
        {
            return Get(keyPrefix, key, false, true);
        }
        public static string GetAppSettingOrConnectString(string keyPrefix, string key)
        {

            if (string.IsNullOrEmpty(keyPrefix)) return null;
            if (string.IsNullOrEmpty(key)) return null;
            var value = Get(keyPrefix, key, false, true);
            if (string.IsNullOrEmpty(value))
            {
                value = Get(keyPrefix, key, false, false);
            }

            return value;
        }
    }
}
