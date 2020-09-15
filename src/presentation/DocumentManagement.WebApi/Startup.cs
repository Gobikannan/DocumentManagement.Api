using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using DocumentManagement.Application;
using DocumentManagement.Common.Config;
using DocumentManagement.Persistence;
using System.Reflection;
using System.Threading.Tasks;
using DocumentManagement.WebApi.Extensions;

namespace DocumentManagement.WebApi
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_musicalogAllowSpecificOrigins";

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    // Added localhost dev ursl and a dummy url for prod
                    builder.WithOrigins("https://localhost:44348", "https://localhost:4200", "https://localhost:3000",
                        "https://enter-your-app-services-name.azurewebsites.net").AllowAnyHeader().AllowAnyMethod();
                });
            });

            // configure strongly typed settings objects
            var storageAccountSettings = Configuration.GetSection("StorageAccountSettings");
            services.Configure<StorageAccountSettings>(storageAccountSettings);
            var appSettings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettings);

            services.AddApplication(Configuration);
            services.AddInfrastructure(Configuration);

            services.AddControllers();
            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.IncludeXmlComments(string.Format(@"{0}\DocumentManagement.xml", System.AppDomain.CurrentDomain.BaseDirectory));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Document Management",
                });

            });
            #endregion
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureExceptionHandler(env);

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.Use((ctx, next) =>
            {
                ctx.Response.Headers.Add("Access-Control-Allow-Origin", ctx.Request.Headers["Origin"]);
                ctx.Response.Headers.Add("Access-Control-Allow-Methods", "*");
                ctx.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                ctx.Response.Headers.Add("Access-Control-Allow-Headers", "AccessToken,Content-Type");
                ctx.Response.Headers.Add("Access-Control-Expose-Headers", "*");
                if (ctx.Request.Method.ToLower() == "options")
                {
                    ctx.Response.StatusCode = 204;

                    return Task.CompletedTask;
                }
                return next();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Swagger
            // this enables middleware to serve auto-generated Swagger as a JSON endpoint
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocumentManagement");
            });
            #endregion
        }
    }
}
