using AutoMapper;
using DocumentManagement.Domain.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Documents.Queries.FetchAllDocuments
{
    public class FetchAllDocumentsHandler : IRequestHandler<FetchAllDocumentsQuery, FetchAllDocumentsResult>
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IMapper mapper;

        public FetchAllDocumentsHandler(IDocumentRepository documentRepository, IMapper mapper)
        {
            this.documentRepository = documentRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<FetchAllDocumentsResult> Handle(FetchAllDocumentsQuery request, CancellationToken cancellationToken)
        {
            FetchAllDocumentsResult result = new FetchAllDocumentsResult();
            var documents = await this.documentRepository.FetchAllDocuments();
            result.Documents = this.mapper.Map<List<FetchDocumentDetail>>(documents);
            return result;
        }
    }
}
