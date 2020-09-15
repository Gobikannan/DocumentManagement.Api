using System;
using System.Threading.Tasks;
using Xunit;
using DocumentManagement.Application.Files.Commands.UploadFile;
using DocumentManagement.Application.Files.Commands.DownloadFile;
using FluentValidation;

namespace DocumentManagement.Application.IntegrationTests.Files.Commands
{
    using static TestFixture;

    public class UploadFileCommandTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task ShouldUploadFile()
        {
            const string data = "R0lGODlhAQABAIAAAAAAAAAAACH5BAAAAAAALAAAAAABAAEAAAICTAEAOw==";

            byte[] bytes = Convert.FromBase64String(data);

            var command = new UploadFileCommand
            {
                Name = "hello.pdf",
                MimeType = "application/pdf",
                File = bytes

            };
            var location = await SendAsync(command);

            var result = await SendAsync(new DownloadFileCommand { Location = location });
            Assert.NotNull(result);
            Assert.Equal(bytes, result.File);
        }

        [Theory]
        [InlineData("", "pdf")]
        [InlineData("HeyPdf", "")]
        public async Task ShouldThrowErrorForInvalidInput(string fileName, string mimeType)
        {
            var command = new UploadFileCommand
            {
                Name = fileName,
                MimeType = mimeType
            };
            await Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
        }
    }
}
