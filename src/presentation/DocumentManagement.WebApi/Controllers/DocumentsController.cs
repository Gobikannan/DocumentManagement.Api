using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DocumentManagement.Application.Files.Commands.UploadFile;
using System.IO;
using DocumentManagement.Application.Documents.Commands.AddNewDocument;
using DocumentManagement.Common.Exceptions;
using DocumentManagement.Application.Documents.Queries.FetchAllDocuments;
using DocumentManagement.Common.Config;
using Microsoft.Extensions.Options;
using DocumentManagement.Application.Documents.Queries.FetchDocumentLocation;
using System;
using DocumentManagement.Application.Files.Commands.DownloadFile;
using DocumentManagement.Application.Files.Commands.DeleteFile;
using DocumentManagement.Application.Documents.Commands.DeleteDocument;

namespace DocumentManagement.WebApi.Controllers
{
    /// <summary>
    /// ApiController for Document Management related api calls
    /// </summary>
    public class DocumentsController : ApiController
    {
        private const string FileTypeAllowed = "application/pdf";
        private readonly AppSettings appSettings;

        /// <summary>
        /// 
        /// </summary>
        public DocumentsController(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        /// <summary>
        /// Adds new document
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UploadDoument()
        {
            if (Request.Form.Files.Count == 0)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotAcceptable, "Please choose a file");
            }
            // take first file as it is supported for single file upload for now, we can extend this for next release
            var formFile = Request.Form.Files[0];
            // validate if the file is pdf
            if (formFile.ContentType != FileTypeAllowed)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotAcceptable, "Invalid file type. Please choose only pdf");
            }

            // validate if the file size is not more than max file size in mb given in appSettings
            var sizeInMb = formFile.Length / 1024 / 1024;
            var maxSize = appSettings.UploadMaxFileSizeInMb > 0 ? appSettings.UploadMaxFileSizeInMb : 5; // default to size 5MB
            if (sizeInMb > maxSize)
            {
                throw new CustomException(System.Net.HttpStatusCode.RequestEntityTooLarge, $"File size must not exceed {maxSize}MB");
            }

            // upload file
            var newFileRequest = new UploadFileCommand();
            
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                newFileRequest.File = memoryStream.ToArray();
            }
            newFileRequest.Name = formFile.FileName;
            newFileRequest.MimeType = FileTypeAllowed;
            var documentUri = await Mediator.Send(newFileRequest);

            // add file to document
            var newDocumentRequest = new AddNewDocumentCommand
            {
                Name = string.IsNullOrEmpty(formFile.Name) ? formFile.FileName : formFile.Name,
                Location = documentUri,
                Size = formFile.Length
            };

            await Mediator.Send(newDocumentRequest);

            return Ok();
        }

        /// <summary>
        /// Fetch All Documents
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> FetchAllDocuments()
        {
            var result = await Mediator.Send(new FetchAllDocumentsQuery());
            return Ok(result.Documents);
        }

        /// <summary>
        /// Fetch All Documents
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadDocument(string id)
        {
            // get file location
            var documentLocaionResult = await Mediator.Send(new FetchDocumentLocationQuery { RowKey = Guid.Parse(id) });
            var documentLocation = documentLocaionResult.DocumentLocation;

            // download it from blob
            var attachment = await Mediator.Send(new DownloadFileCommand { Location = documentLocation });

            return File(attachment.File, FileTypeAllowed);
        }

        /// <summary>
        /// Fetch All Documents
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{rowKey}")]
        public async Task<IActionResult> DeleteDocument(Guid rowKey)
        {
            // get file location
            var documentLocaionResult = await Mediator.Send(new FetchDocumentLocationQuery { RowKey = rowKey });
            var documentLocation = documentLocaionResult.DocumentLocation;

            // download it from blob
            await Mediator.Send(new DeleteFileCommand { FileName = documentLocation });

            await Mediator.Send(new DeleteDocumentCommand { RowKey = rowKey });

            return Ok();
        }
    }
}
