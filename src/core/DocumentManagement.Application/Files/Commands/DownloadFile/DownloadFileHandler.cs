using DocumentManagement.Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.Application.Files.Commands.DownloadFile
{
    public class DownloadFileHandler : IRequestHandler<DownloadFileCommand, DownloadFileResult>
    {
        private readonly IDownloadFileRepository downloadFileRepository;

        public DownloadFileHandler(IDownloadFileRepository downloadFileRepository)
        {
            this.downloadFileRepository = downloadFileRepository;
        }

        public async Task<DownloadFileResult> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
        {
            var result = new DownloadFileResult();
            var document = await this.downloadFileRepository.DownloadDocument(request.Location);
            result.File = document;
            return result;
        }
    }
}
