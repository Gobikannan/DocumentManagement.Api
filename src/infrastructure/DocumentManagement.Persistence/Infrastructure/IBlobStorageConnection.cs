using Microsoft.Azure.Storage.Blob;
using System.Threading.Tasks;

namespace DocumentManagement.Persistence
{
    public interface IBlobStorageConnection
    {
        Task<CloudBlobClient> GetCloudBlobClient(string containerName);
    }
}
