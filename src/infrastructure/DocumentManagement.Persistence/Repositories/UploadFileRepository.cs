using DocumentManagement.Common.Config;
using DocumentManagement.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;

namespace DocumentManagement.Persistence.Repositories
{
    public class UploadFileRepository : IUploadFileRepository
    {
        private readonly IBlobStorageConnection storageConnection;
        private readonly StorageAccountSettings storageAccountSettings;

        public UploadFileRepository(IBlobStorageConnection storageConnection, IOptions<StorageAccountSettings> storageAccountSettings)
        {
            this.storageConnection = storageConnection;
            this.storageAccountSettings = storageAccountSettings.Value;
        }

        public async Task<string> UploadDocument(byte[] document, string documentName, string documentMimeType)
        {
            var client = await this.storageConnection.GetCloudBlobClient(this.storageAccountSettings.ContainerName);
            var container = client.GetContainerReference(this.storageAccountSettings.ContainerName);

            //find the extension and append it to the unique file name below
            var lastIndexOfDot = documentName.LastIndexOf(".");
            var extension = documentName.Substring(lastIndexOfDot, documentName.Length - lastIndexOfDot);

            //unique file name
            var fileName = Guid.NewGuid().ToString() + extension;
            var cloudBlockBlob = container.GetBlockBlobReference(fileName);
            cloudBlockBlob.Properties.ContentType = documentMimeType;
            
            // add metadata to the blob file
            cloudBlockBlob.Metadata.Add("Name", documentName);

            //upload
            await cloudBlockBlob.UploadFromByteArrayAsync(document, 0, document.Length);

            //return the the uri 
            return cloudBlockBlob.Uri.AbsoluteUri;
        }
    }
}
