using System;

namespace DocumentManagement.Domain.Entities
{
    public class Document : BaseEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public long Size { get; set; }
        public int Order { get; set; }

        public Document()
        {
            this.PartitionKey = Guid.NewGuid().ToString();
            this.RowKey = Guid.NewGuid().ToString();
        }
    }
}
