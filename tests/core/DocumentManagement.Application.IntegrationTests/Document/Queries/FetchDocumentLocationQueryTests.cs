using DocumentManagement.Application.Documents.Commands.AddNewDocument;
using System;
using System.Threading.Tasks;
using Xunit;
using DocumentManagement.Application.Documents.Queries.FetchDocumentLocation;
using FluentValidation;

namespace DocumentManagement.Application.IntegrationTests.Document.Queries
{
    using static TestFixture;

    public class FetchDocumentLocationQueryTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task ShouldFetchDocumentLocation()
        {
            var name = "ITestFetch-" + DateTime.Now.ToString();
            var command = new AddNewDocumentCommand
            {
                Name = name,
                Location = Guid.NewGuid().ToString(),
                Size = 1234

            };
            var rowKey = await SendAsync(command);

            var query = new FetchDocumentLocationQuery { RowKey = rowKey };
            var result = await SendAsync(query);
            Assert.NotNull(result);
            var documentLocation = result.DocumentLocation;
            Assert.Equal(command.Location, documentLocation);
        }

        [Fact]
        public async Task ShouldThrowErrorForInvalidInput()
        {
            var query = new FetchDocumentLocationQuery { RowKey = Guid.Empty };
            await Assert.ThrowsAsync<ValidationException>(() => SendAsync(query));
        }
    }
}
