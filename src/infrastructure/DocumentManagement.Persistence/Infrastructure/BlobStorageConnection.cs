using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Threading.Tasks;

namespace DocumentManagement.Persistence
{
    public class BlobStorageConnection : IBlobStorageConnection
    {
        private readonly string connectionString;

        public BlobStorageConnection(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<CloudBlobClient> GetCloudBlobClient(string containerName)
        {
            if (CloudStorageAccount.TryParse(connectionString, out CloudStorageAccount storageAccount))
            {
                var client = storageAccount.CreateCloudBlobClient();
                var container = client.GetContainerReference(containerName);
                if (await container.CreateIfNotExistsAsync())
                {
                    await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                }
                return client;
            }

            throw new Exception("A connection string has not been defined in the appSettings. ");
        }
    }
}
