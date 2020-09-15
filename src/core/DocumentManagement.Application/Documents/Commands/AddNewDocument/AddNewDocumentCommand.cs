using MediatR;
using System;

namespace DocumentManagement.Application.Documents.Commands.AddNewDocument
{
    public class AddNewDocumentCommand : DocumentDetail, IRequest<Guid>
    {

    }
}
