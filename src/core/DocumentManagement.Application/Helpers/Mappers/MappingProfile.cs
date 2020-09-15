using AutoMapper;
using Microsoft.Extensions.Configuration;
using DocumentManagement.Domain.Entities;
using DocumentManagement.Application.Documents.Commands.AddNewDocument;
using DocumentManagement.Application.Documents.Queries.FetchAllDocuments;

namespace DocumentManagement.Application.Helpers.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile(IConfiguration configuration)
        {
            CreateMap<AddNewDocumentCommand, Document>();
            CreateMap<Document, FetchDocumentDetail>();
        }
    }
}
