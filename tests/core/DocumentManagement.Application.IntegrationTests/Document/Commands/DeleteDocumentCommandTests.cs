using FluentValidation;
using DocumentManagement.Application.Documents.Commands.AddNewDocument;
using System;
using System.Threading.Tasks;
using Xunit;
using DocumentManagement.Application.Documents.Queries.FetchDocumentLocation;
using DocumentManagement.Application.Documents.Commands.DeleteDocument;
using DocumentManagement.Common.Exceptions;

namespace DocumentManagement.Application.IntegrationTests.Document.Commands
{
    using static TestFixture;

    public class DeleteDocumentCommandTests : IClassFixture<TestFixture>
    {
        private async Task<Guid> CreateTestData()
        {
            var name = "ITestDelete-" + DateTime.Now.ToString();
            var command = new AddNewDocumentCommand
            {
                Name = name,
                Location = "test",
                Size = 1234

            };
            return await SendAsync(command);
        }

        [Fact]
        public async Task ShouldDeleteNewDocument()
        {
            var rowKey = await CreateTestData();
            var command = new DeleteDocumentCommand
            {
                RowKey = rowKey
            };
            await SendAsync(command);

            await Assert.ThrowsAsync<CustomException>(() => SendAsync(new FetchDocumentLocationQuery { RowKey = rowKey }));
        }

        [Fact]
        public async Task ShouldThrowErrorForInvalidInput()
        {
            var command = new DeleteDocumentCommand
            {
                RowKey = Guid.Empty
            };
            await Assert.ThrowsAsync<ValidationException>(() => SendAsync(command));
        }
    }
}
