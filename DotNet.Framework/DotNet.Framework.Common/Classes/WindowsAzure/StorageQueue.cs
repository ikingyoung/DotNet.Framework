using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Classes.WindowsAzure
{
    public class StorageQueue
    {

        private readonly CloudQueue _innerQueue;

        public StorageQueue(string queueName)
        {
            try
            {
                CloudStorageAccount storageAccount = StorageAccountConfig.GetBackendStorageAccount();
                CloudQueueClient queueStorage = storageAccount.CreateCloudQueueClient();

                // Add Retry Policy to Queue Client
                //queueStorage.RetryPolicy = Configration.StorageRetryConfig.StorageRetryPolicy;

                _innerQueue = queueStorage.GetQueueReference(queueName);
                _innerQueue.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CloudQueueMessage DeQueue()
        {
            return _innerQueue.GetMessage();
        }
        public IEnumerable<CloudQueueMessage> DeQueue(int count)
        {
            //var i = _innerQueue.GetMessage();
            return _innerQueue.GetMessages(count);
        }

        public void EnQueue(string message)
        {
            _innerQueue.AddMessage(new CloudQueueMessage(message));
        }

        public void Remove(CloudQueueMessage cqm)
        {
            _innerQueue.DeleteMessage(cqm);
        }


        //public void EnQueue(BackendActionQueueItem action)
        //{
        //    byte[] content = SerializerHelper.FormatterObjectToBinary(action);
        //    _innerQueue.AddMessage(new CloudQueueMessage(content));
        //    if (CountersInstalled)
        //    {
        //        CounterActionQueueEnQueueCount.Increment();
        //        CounterActionQueueEnQueueCountPerSecond.Increment();
        //    }
        //}

        //public BackendActionQueueItem DeQueueAndDelete()
        //{
        //    CloudQueueMessage message = _innerQueue.GetMessage();
        //    if (message == null) return null;

        //    byte[] content = message.AsBytes;
        //    try
        //    {
        //        var action = SerializerHelper.FormatterBinaryToObject<BackendActionQueueItem>(content);
        //        action.MessageId = message.Id;
        //        action.MessagePopReceipt = message.PopReceipt;

        //        return action;
        //    }
        //    catch (Exception ex)
        //    {
        //        //如果发生异常，例如序列化异常，则直接删除Message,避免重复发生
        //        Logger.WriteErrorLog(LogSourceName, "ActionQueue DeQueueAndDelete Exception.", ex);
        //        throw;
        //    }
        //    finally
        //    {
        //        _innerQueue.DeleteMessage(message);
        //        if (CountersInstalled)
        //        {
        //            CounterActionQueueDeQueueCount.Increment();
        //            CounterActionQueueDeQueueCountPerSecond.Increment();
        //        }
        //    }
        //}

        //public List<BackendActionQueueItem> DeQueueAndDelete(int numberOfMessage)
        //{
        //    var messagesToGet = numberOfMessage > 32 ? 32 : numberOfMessage;
        //    var messages = _innerQueue.GetMessages(messagesToGet);
        //    var result = new ConcurrentQueue<BackendActionQueueItem>();
        //    if (messages == null)
        //    {
        //        return new List<BackendActionQueueItem>();
        //    }
        //    messages.AsParallel().ForEach(message =>
        //    {
        //        try
        //        {
        //            var action = SerializerHelper.FormatterBinaryToObject<BackendActionQueueItem>(message.AsBytes);
        //            action.MessageId = message.Id;
        //            action.MessagePopReceipt = message.PopReceipt;
        //            result.Enqueue(action);
        //        }
        //        catch (Exception ex)
        //        {
        //            //如果发生异常，例如序列化异常，则直接删除Message,避免重复发生
        //            Logger.WriteErrorLog(LogSourceName, "ActionQueue DeQueueAndDelete Exception.", ex);
        //            throw;
        //        }
        //        finally
        //        {
        //            _innerQueue.DeleteMessage(message);
        //            Logger.WriteDebugLog(LogSourceName, "delet message id:" + message.Id);
        //        }
        //    });
        //    var list = new List<BackendActionQueueItem>(result.ToArray());
        //    if (CountersInstalled && list.Count > 0)
        //    {
        //        CounterActionQueueDeQueueCount.IncrementBy(list.Count);
        //        CounterActionQueueDeQueueCountPerSecond.IncrementBy(list.Count);
        //    }
        //    return list;
        //}
    }
}
