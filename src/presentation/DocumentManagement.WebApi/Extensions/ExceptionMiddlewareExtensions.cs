using DocumentManagement.Common.ExceptionHelperModel;
using DocumentManagement.Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace DocumentManagement.WebApi.Extensions
{
    /// <summary>
    /// ExceptionMiddlewareExtensions
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// ConfigureExceptionHandler
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        string message = env.IsDevelopment() ? contextFeature.Error.Message : "Internal Server Error";
                        if (contextFeature.Error is CustomException)
                        {
                            context.Response.StatusCode = (int)(contextFeature.Error as CustomException).StatusCode;
                            message = contextFeature.Error.Message;
                        }

                        string error = new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = message
                        }.ToString();
                        await context.Response.WriteAsync(error);
                    }
                });
            });
        }
    }
}
