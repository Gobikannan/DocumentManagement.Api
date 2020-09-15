using DocumentManagement.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Documents.Queries.FetchDocumentLocation
{
    public class FetchDocumentLocationHandler : IRequestHandler<FetchDocumentLocationQuery, FetchDocumentLocationResult>
    {
        private readonly IDocumentRepository documentRepository;

        public FetchDocumentLocationHandler(IDocumentRepository documentRepository)
        {
            this.documentRepository = documentRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FetchDocumentLocationResult> Handle(FetchDocumentLocationQuery request, CancellationToken cancellationToken)
        {
            var result = new FetchDocumentLocationResult();
            result.DocumentLocation = await this.documentRepository.FetchDocumentLocation(request.RowKey);
            return result;
        }
    }
}
