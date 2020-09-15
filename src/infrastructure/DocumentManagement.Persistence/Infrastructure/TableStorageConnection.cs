using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.Persistence
{
    public class TableStorageConnection : ITableStorageConnection
    {
        private readonly string connectionString;

        public TableStorageConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<CloudTableClient> GetCloudTableClient(string containerName, string tableName)
        {
            if (CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                var client = storageAccount.CreateCloudTableClient();

                var table = client.GetTableReference(tableName);

                await table.CreateIfNotExistsAsync();

                return client;
            }

            throw new Exception("A connection string has not been defined in the appSettings/KeyVault/Secret.json. ");
        }
    }
}
