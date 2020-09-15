using MediatR;

namespace DocumentManagement.Application.Files.Commands.UploadFile
{
    public class UploadFileCommand : IRequest<string>
    {
        public string Name { get; set; }
        public string MimeType { get; set; }
        public byte[] File { get; set; }
    }
}
