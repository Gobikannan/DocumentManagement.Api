using DocumentManagement.Common.Config;
using DocumentManagement.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Linq;
using System.IO;

namespace DocumentManagement.Persistence.Repositories
{
    public class DownloadFileRepository : IDownloadFileRepository
    {
        private readonly IBlobStorageConnection storageConnection;
        private readonly StorageAccountSettings storageAccountSettings;

        public DownloadFileRepository(IBlobStorageConnection storageConnection, IOptions<StorageAccountSettings> storageAccountSettings)
        {
            this.storageConnection = storageConnection;
            this.storageAccountSettings = storageAccountSettings.Value;
        }

        public async Task<byte[]> DownloadDocument(string fileName)
        {
            var client = await this.storageConnection.GetCloudBlobClient(this.storageAccountSettings.ContainerName);
            var container = client.GetContainerReference(this.storageAccountSettings.ContainerName);

            var blockBlob = container.GetBlockBlobReference(fileName.Split("/").Last());
            using (var memoryStream = new MemoryStream())
            {
                await blockBlob.DownloadToStreamAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
