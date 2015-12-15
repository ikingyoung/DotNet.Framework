using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Classes.WindowsAzure
{
    public static class StorageAccountConfig
    {
        public enum StorageSection
        {
            Frontend,
            Backend,
            CarData,
            Log
        }

        // Storage Config Key in Cloud configuration.
        private const string ConfigurationKeyFrontend = "FrontendStorageAccount";
        private const string ConfigurationKeyBackend = "BackendStorageAccount";
        private const string ConfigurationKeyCarData = "CarDataStorageAccount";
        private const string ConfigurationKeyLog = "LogStorageAccount";

        /// <summary>
        /// Get Storage Account by specified Section
        /// </summary>
        /// <param name="section">Storage Account Section</param>
        /// <returns></returns>
        public static CloudStorageAccount GetStorageAccount(StorageSection section)
        {
            string configkey;
            switch (section)
            {
                case StorageSection.Frontend:
                    configkey = ConfigurationKeyFrontend;
                    break;
                case StorageSection.Backend:
                    configkey = ConfigurationKeyBackend;
                    break;
                case StorageSection.CarData:
                    configkey = ConfigurationKeyCarData;
                    break;
                case StorageSection.Log:
                    configkey = ConfigurationKeyLog;
                    break;
                default:
                    throw new InvalidOperationException("Unknown Storage Section");
            }
            var account = CloudConfigurationManager.GetSetting(configkey);
            return CloudStorageAccount.Parse(account);
        }

        /// <summary>
        /// Get Frontend Storage Account 
        /// </summary>
        /// <returns></returns>
        public static CloudStorageAccount GetFrontendStorageAccount()
        {
            return GetStorageAccount(StorageSection.Frontend);
        }

        /// <summary>
        /// Get Backend Storage Account
        /// </summary>
        /// <returns></returns>
        public static CloudStorageAccount GetBackendStorageAccount()
        {
            return GetStorageAccount(StorageSection.Backend);
        }

        /// <summary>
        /// Get Car Data Storage Account
        /// </summary>
        /// <returns></returns>
        public static CloudStorageAccount GetCarDataStorageAccount()
        {
            return GetStorageAccount(StorageSection.CarData);
        }

        public static CloudStorageAccount GetLogStorageAccount()
        {
            return GetStorageAccount(StorageSection.Log);
        }

    }
}
