using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace DocumentManagement.Persistence
{
    public interface ITableStorageConnection
    {
        Task<CloudTableClient> GetCloudTableClient(string containerName, string tableName);
    }
}
