using FluentValidation;

namespace DocumentManagement.Application.Files.Commands.DeleteFile
{
    public class DeleteFileValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileValidator()
        {
            RuleFor(v => v.FileName).NotEmpty().WithMessage("File name is required.");
        }
    }
}
