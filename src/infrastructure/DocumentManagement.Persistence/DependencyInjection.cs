using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DocumentManagement.Domain.Interfaces;
using DocumentManagement.Persistence.Repositories;

namespace DocumentManagement.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var dbConnectionString = configuration.GetConnectionString("StorageAccountConnection");
            services.AddTransient<IBlobStorageConnection, BlobStorageConnection>(provider => new BlobStorageConnection(dbConnectionString));
            services.AddTransient<ITableStorageConnection, TableStorageConnection>(provider => new TableStorageConnection(dbConnectionString));
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IUploadFileRepository, UploadFileRepository>();
            services.AddScoped<IDownloadFileRepository, DownloadFileRepository>();
            services.AddScoped<IDeleteFileRepository, DeleteFileRepository>();            

            return services;
        }
    }
}
