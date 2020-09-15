using FluentValidation;

namespace DocumentManagement.Application.Documents.Commands.DeleteDocument
{
    public class DeleteDocumentValidator : AbstractValidator<DeleteDocumentCommand>
    {
        public DeleteDocumentValidator()
        {
            RuleFor(v => v.RowKey).NotEmpty().WithMessage("RowKey is required.");
        }
    }
}
