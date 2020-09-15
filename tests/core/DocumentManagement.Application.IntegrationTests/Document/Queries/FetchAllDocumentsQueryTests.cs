using DocumentManagement.Application.Documents.Commands.AddNewDocument;
using System;
using System.Threading.Tasks;
using Xunit;
using DocumentManagement.Application.Documents.Queries.FetchAllDocuments;
using System.Linq;

namespace DocumentManagement.Application.IntegrationTests.Document.Queries
{
    using static TestFixture;

    public class FetchAllDocumentsQueryTests : IClassFixture<TestFixture>
    {
        [Fact]
        public async Task ShouldFetchAllDocuments()
        {
            var name = "ITestFetchAll-" + DateTime.Now.ToString();
            var command = new AddNewDocumentCommand
            {
                Name = name,
                Location = Guid.NewGuid().ToString(),
                Size = 1234

            };
            await SendAsync(command);
            
            var query = new FetchAllDocumentsQuery();
            var result = await SendAsync(query);
            var document = result.Documents.FirstOrDefault(x => x.Name == name);
            Assert.NotNull(document);
            Assert.Equal(command.Name, document.Name);
            Assert.Equal(command.Location, document.Location);
            Assert.Equal(command.Size, document.Size);
        }
    }
}
