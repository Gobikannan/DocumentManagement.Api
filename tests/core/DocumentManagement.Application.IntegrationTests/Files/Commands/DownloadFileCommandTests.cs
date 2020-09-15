using System;
using System.Threading.Tasks;
using Xunit;
using DocumentManagement.Application.Files.Commands.UploadFile;
using DocumentManagement.Application.Files.Commands.DownloadFile;
using FluentValidation;

namespace DocumentManagement.Application.IntegrationTests.Files.Commands
{
    using static TestFixture;

    public class DownloadFileCommandTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task ShouldDownloadFile()
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

        [Fact]
        public async Task ShouldThrowErrorForInvalidInput()
        {
            var command = new DownloadFileCommand
            {
                Location = ""
            };
            await Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
        }
    }
}
