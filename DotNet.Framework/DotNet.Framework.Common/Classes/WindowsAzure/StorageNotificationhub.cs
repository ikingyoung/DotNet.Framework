using DotNet.Framework.Common.Classes.Cache;
using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Classes.WindowsAzure
{
    public class StorageNotificationhub
    {
        public static NotificationHubClient GetInstance(string notificationHubName, string notificationHubConnectstring)
        {
            var value = GlobalCache.get(notificationHubName);
            if (value == null)
            {
                value = NotificationHubClient.CreateClientFromConnectionString(notificationHubConnectstring, notificationHubName);
                GlobalCache.set(notificationHubName, value);
                return (NotificationHubClient)(object)value;
            }

            return (NotificationHubClient)(object)value;
        }

    }
}
