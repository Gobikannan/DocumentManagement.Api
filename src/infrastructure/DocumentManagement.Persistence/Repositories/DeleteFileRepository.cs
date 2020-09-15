using DocumentManagement.Common.Config;
using DocumentManagement.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Linq;

namespace DocumentManagement.Persistence.Repositories
{
    public class DeleteFileRepository : IDeleteFileRepository
    {
        private readonly IBlobStorageConnection storageConnection;
        private readonly StorageAccountSettings storageAccountSettings;

        public DeleteFileRepository(IBlobStorageConnection storageConnection, IOptions<StorageAccountSettings> storageAccountSettings)
        {
            this.storageConnection = storageConnection;
            this.storageAccountSettings = storageAccountSettings.Value;
        }

        public async Task DeleteFile(string fileName)
        {
            var client = await this.storageConnection.GetCloudBlobClient(this.storageAccountSettings.ContainerName);
            var container = client.GetContainerReference(this.storageAccountSettings.ContainerName);

            var blockBlob = container.GetBlockBlobReference(fileName.Split("/").Last());
            await blockBlob.DeleteAsync();
        }
    }
}
