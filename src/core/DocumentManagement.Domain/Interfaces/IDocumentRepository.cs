using DocumentManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.Domain.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document> AddNewDocument(Document document);
        Task<List<Document>> FetchAllDocuments();
        Task<string> FetchDocumentLocation(Guid rowKey);
        Task DeleteDocument(Guid rowKey);
        Task<Document> FetchDocument(Guid rowKey);
    }
}
