using MediatR;
using DocumentManagement.Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Files.Commands.UploadFile
{
    public class UploadFileHandler : IRequestHandler<UploadFileCommand, string>
    {
        private readonly IUploadFileRepository uploadFileRepository;

        public UploadFileHandler(IUploadFileRepository uploadFileRepository)
        {
            this.uploadFileRepository = uploadFileRepository;
        }

        public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            var result = await this.uploadFileRepository.UploadDocument(request.File, request.Name, request.MimeType);

            return result;
        }
    }
}
