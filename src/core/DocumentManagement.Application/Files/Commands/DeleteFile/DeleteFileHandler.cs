using MediatR;
using DocumentManagement.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Files.Commands.DeleteFile
{
    public class DeleteFileHandler : IRequestHandler<DeleteFileCommand>
    {
        private readonly IDeleteFileRepository deleteFileRepository;

        public DeleteFileHandler(IDeleteFileRepository deleteFileRepository)
        {
            this.deleteFileRepository = deleteFileRepository;
        }

        public async Task<Unit> Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            await this.deleteFileRepository.DeleteFile(request.FileName);

            return Unit.Value;
        }
    }
}
