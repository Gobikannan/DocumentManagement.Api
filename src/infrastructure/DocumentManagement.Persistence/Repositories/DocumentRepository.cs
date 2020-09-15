using DocumentManagement.Common.Config;
using DocumentManagement.Domain.Entities;
using DocumentManagement.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System;
using System.Linq;
using DocumentManagement.Common.Exceptions;

namespace DocumentManagement.Persistence.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ITableStorageConnection tableStorageConnection;
        private readonly StorageAccountSettings storageAccountSettings;

        public DocumentRepository(ITableStorageConnection tableStorageConnection, IOptions<StorageAccountSettings> storageAccountSettings)
        {
            this.tableStorageConnection = tableStorageConnection;
            this.storageAccountSettings = storageAccountSettings.Value;
        }

        public async Task<Document> AddNewDocument(Document document)
        {
            var client = await this.tableStorageConnection.GetCloudTableClient(this.storageAccountSettings.ContainerName, this.storageAccountSettings.TableName);
            var table = client.GetTableReference(this.storageAccountSettings.TableName);

            var insertOperation = TableOperation.InsertOrMerge(document);

            var result = await table.ExecuteAsync(insertOperation);

            return result.Result as Document;
        }

        public async Task<List<Document>> FetchAllDocuments()
        {
            var records = new List<Document>();
            var client = await this.tableStorageConnection.GetCloudTableClient(this.storageAccountSettings.ContainerName, this.storageAccountSettings.TableName);
            var table = client.GetTableReference(this.storageAccountSettings.TableName);

            var query = new TableQuery<Document>();

            TableContinuationToken token = null;
            do
            {
                var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                records.AddRange(resultSegment.Results);
            } while (token != null);

            return records;
        }

        public async Task<Document> FetchDocument(Guid rowKey)
        {
            var records = new List<Document>();
            var client = await this.tableStorageConnection.GetCloudTableClient(this.storageAccountSettings.ContainerName, this.storageAccountSettings.TableName);
            var table = client.GetTableReference(this.storageAccountSettings.TableName);

            var query = new TableQuery<Document>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey.ToString()));

            TableContinuationToken token = null;
            do
            {
                var resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                records.AddRange(resultSegment.Results);
            } while (token != null);

            var record = records.SingleOrDefault();
            if (record == null)
            {
                throw new CustomException(System.Net.HttpStatusCode.NotFound, "Document not found");
            }

            return record;
        }

        public async Task<string> FetchDocumentLocation(Guid rowKey)
        {
            var record = await FetchDocument(rowKey);

            return record.Location;
        }

        public async Task DeleteDocument(Guid rowKey)
        {
            var client = await this.tableStorageConnection.GetCloudTableClient(this.storageAccountSettings.ContainerName, this.storageAccountSettings.TableName);
            var table = client.GetTableReference(this.storageAccountSettings.TableName);

            var entity = await FetchDocument(rowKey);

            TableOperation delteOperation = TableOperation.Delete(entity);
            await table.ExecuteAsync(delteOperation);

        }
    }
}
