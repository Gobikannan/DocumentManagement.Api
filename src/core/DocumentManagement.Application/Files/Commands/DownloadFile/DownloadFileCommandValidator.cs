using FluentValidation;

namespace DocumentManagement.Application.Files.Commands.DownloadFile
{
    public class DownloadFileCommandValidator : AbstractValidator<DownloadFileCommand>
    {
        public DownloadFileCommandValidator()
        {
            RuleFor(v => v.Location).NotEmpty().WithMessage("Location is required.");
        }
    }
}
