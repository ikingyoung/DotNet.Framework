using DotNet.Framework.Common.Classes.Cache;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Framework.Common.Classes.WindowsAzure
{
    public class StorageTableContextBase
    {
        private CloudTable _innerTable;
        public CloudTable ActionTable
        {
            get
            {
                return _innerTable;
            }
        }
        public StorageTableContextBase(CloudStorageAccount account, string tableName)
        {

            var tableClient = account.CreateCloudTableClient();
            _innerTable = tableClient.GetTableReference(tableName);
            _innerTable.CreateIfNotExists();
        }

        public void ExcuteOperationAsync(TableOperation operation, TableRequestOptions requestOptions = null)
        {
            if (requestOptions == null)
                Task<TableResult>.Factory.FromAsync(this.ActionTable.BeginExecute, this.ActionTable.EndExecute, operation, null);
            else
                Task<TableResult>.Factory.FromAsync(this.ActionTable.BeginExecute, this.ActionTable.EndExecute, operation, requestOptions);
        }
    }
    public class StorageTableContext<T> : StorageTableContextBase where T : TableEntity
    {
        public StorageTableContext(CloudStorageAccount account) : base(account, typeof(T).Name) { }
        public StorageTableContext(string connectString) : this(CloudStorageAccount.Parse(connectString)) { }
        public void Insert(T entity, bool isSyncExcute = true)
        {
            var insertOperation = TableOperation.Insert(entity);
            if (isSyncExcute)
                ActionTable.Execute(insertOperation);
            else
                ActionTable.ExecuteAsync(insertOperation);
        }
        public T Get(string partitionKey, string rowkey)
        {
            var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowkey);
            var result = ActionTable.Execute(retrieveOperation).Result;

            return result == null ? default(T) : (T)(object)result;
        }
        public void InsertOrReplace(T entity, bool isSyncExcute = true)
        {
            var updateOperation = TableOperation.InsertOrReplace(entity);
            if (isSyncExcute)
                ActionTable.Execute(updateOperation);
            else
                ActionTable.ExecuteAsync(updateOperation);
        }

    }
    public class StorageTableContextHelper
    {
        public static TStorageTableContext GetInstance<TStorageTableContext, TTableEntity>(params object[] args)
        {
            var key = typeof(TTableEntity).Name;
            var value = GlobalCache.get(key);
            if (value == null)
            {
                value = Activator.CreateInstance(typeof(TStorageTableContext), args);
                GlobalCache.set(key, value);
                return (TStorageTableContext)(object)value;
            }
            else
            {
                return (TStorageTableContext)(object)value;
            }
        }
        public static dynamic GetInstance(Type typeStorageTableContext, Type typeTableEntity, params object[] args)
        {
            var key = typeStorageTableContext.Name;
            var value = GlobalCache.get(key);
            if (value == null)
            {
                value = Activator.CreateInstance(typeStorageTableContext, args);
                GlobalCache.set(key, value);
            }
            return value;

        }
    }
}
