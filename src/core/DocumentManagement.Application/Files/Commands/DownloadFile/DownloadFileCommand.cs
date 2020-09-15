using MediatR;

namespace DocumentManagement.Application.Files.Commands.DownloadFile
{
    public class DownloadFileCommand : IRequest<DownloadFileResult>
    {
        public string Location { get; set; }
    }
}
