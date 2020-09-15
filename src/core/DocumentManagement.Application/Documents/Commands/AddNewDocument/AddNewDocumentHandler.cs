using AutoMapper;
using MediatR;
using DocumentManagement.Domain.Entities;
using DocumentManagement.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace DocumentManagement.Application.Documents.Commands.AddNewDocument
{
    public class AddNewDocumentHandler : IRequestHandler<AddNewDocumentCommand, Guid>
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IMapper mapper;

        public AddNewDocumentHandler(IDocumentRepository documentRepository, IMapper mapper)
        {
            this.documentRepository = documentRepository;
            this.mapper = mapper;
        }

        public async Task<Guid> Handle(AddNewDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = this.mapper.Map<Document>(request);
            document.Order = 1; // TODO - implement logic to maintain order
            var result = await this.documentRepository.AddNewDocument(document);

            return Guid.Parse(result.RowKey);
        }
    }
}
