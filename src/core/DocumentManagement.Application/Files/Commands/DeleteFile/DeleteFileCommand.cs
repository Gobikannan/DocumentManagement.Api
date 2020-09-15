using MediatR;


namespace DocumentManagement.Application.Files.Commands.DeleteFile
{
    public class DeleteFileCommand : IRequest
    {
        public string FileName { get; set; }
    }
}
