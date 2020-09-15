using System.Threading.Tasks;

namespace DocumentManagement.Domain.Interfaces
{
    public interface IDeleteFileRepository
    {
        Task DeleteFile(string name);
    }
}
