using System.Threading.Tasks;

namespace DocumentManagement.Domain.Interfaces
{
    public interface IDownloadFileRepository
    {
        Task<byte[]> DownloadDocument(string name);
    }
}
