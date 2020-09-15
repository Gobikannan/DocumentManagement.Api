using MediatR;
using System;

namespace DocumentManagement.Application.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentCommand : IRequest
    {
        public Guid RowKey { get; set; }
    }
}
