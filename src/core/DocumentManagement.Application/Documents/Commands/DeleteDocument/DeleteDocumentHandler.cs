using MediatR;
using DocumentManagement.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentHandler : IRequestHandler<DeleteDocumentCommand>
    {
        private readonly IDocumentRepository documentRepository;

        public DeleteDocumentHandler(IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        public async Task<Unit> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            await this.documentRepository.DeleteDocument(request.RowKey);

            return Unit.Value;
        }
    }
}
