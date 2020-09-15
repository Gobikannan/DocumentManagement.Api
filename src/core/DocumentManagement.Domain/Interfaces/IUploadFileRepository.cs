using System.Threading.Tasks;

namespace DocumentManagement.Domain.Interfaces
{
    public interface IUploadFileRepository
    {
        Task<string> UploadDocument(byte[] document, string documentName, string documentMimeType);
    }
}
