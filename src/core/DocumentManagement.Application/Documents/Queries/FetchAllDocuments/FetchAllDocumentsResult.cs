using System;
using System.Collections.Generic;

namespace DocumentManagement.Application.Documents.Queries.FetchAllDocuments
{
    public class FetchAllDocumentsResult
    {
        public List<FetchDocumentDetail> Documents { get; set; }
    }

    public class FetchDocumentDetail : DocumentDetail
    {
        public Guid RowKey { get; set; }
        public int Order { get; set; }
    }
}
