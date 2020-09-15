using FluentValidation;

namespace DocumentManagement.Application.Files.Commands.UploadFile
{
    public class UploadFileValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadFileValidator()
        {
            RuleFor(v => v.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(v => v.MimeType).NotEmpty().WithMessage("MimeType is required.");
        }
    }
}
