using FluentValidation;

namespace DocumentManagement.Application.Documents.Commands.AddNewDocument
{
    public class AddNewDocumentCommandValidator : AbstractValidator<AddNewDocumentCommand>
    {
        public AddNewDocumentCommandValidator()
        {
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(v => v.Location).NotEmpty().WithMessage("Location is required.");
        }
    }
}
