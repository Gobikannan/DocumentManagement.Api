using MediatR;
using System;

namespace DocumentManagement.Application.Documents.Queries.FetchDocumentLocation
{
    public class FetchDocumentLocationQuery : IRequest<FetchDocumentLocationResult>
    {
        public Guid RowKey { get; set; }
    }
}
