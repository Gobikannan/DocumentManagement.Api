using FluentValidation;
using System;
using System.Threading.Tasks;
using Xunit;
using DocumentManagement.Application.Files.Commands.DeleteFile;

namespace DocumentManagement.Application.IntegrationTests.Files.Commands
{
    using static TestFixture;

    public class DeleteFileCommandTests : IClassFixture<TestFixture>
    {
        //[Fact]
        //public async Task ShouldDeleteFile()
        //{
        //    var name = "ITest-" + DateTime.Now.ToString();
        //    var command = new DeleteFileCommand
        //    {
        //        FileName = "hello"

        //    };
        //    await SendAsync(command);

        //    var result = await SendAsync(new FetchDocumentLocationQuery { RowKey = rowKey });
        //    Assert.NotNull(result);
        //    Assert.Equal("test", result.DocumentLocation);
        //}

        [Fact]
        public async Task ShouldThrowErrorForInvalidInput()
        {
            var command = new DeleteFileCommand
            {
                FileName = ""
            };
            await Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
        }
    }
}
