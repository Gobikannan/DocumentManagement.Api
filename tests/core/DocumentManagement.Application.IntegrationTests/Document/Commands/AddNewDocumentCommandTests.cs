using FluentValidation;
using DocumentManagement.Application.Documents.Commands.AddNewDocument;
using System;
using System.Threading.Tasks;
using Xunit;
using DocumentManagement.Application.Documents.Queries.FetchDocumentLocation;

namespace DocumentManagement.Application.IntegrationTests.Document.Commands
{
    using static TestFixture;

    public class AddNewDocumentCommandTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task ShouldCreateNewDocument()
        {
            var name = "ITest-" + DateTime.Now.ToString();
            var command = new AddNewDocumentCommand
            {
                Name = name,
                Location = "test",
                Size = 1234

            };
            var rowKey = await SendAsync(command);

            var result = await SendAsync(new FetchDocumentLocationQuery { RowKey = rowKey });
            Assert.NotNull(result);
            Assert.Equal("test", result.DocumentLocation);
        }

        [Theory]
        [InlineData("", "testlocation")]
        [InlineData("File", "")]
        public async Task ShouldThrowErrorForInvalidInput(string name, string location)
        {
            var command = new AddNewDocumentCommand
            {
                Location = location,
                Name = name
            };
            await Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
        }
    }
}
