using Autofac.Extras.Moq;
using DocumentManagement.Application.Files.Commands.UploadFile;
using DocumentManagement.Common.Config;
using DocumentManagement.Common.Exceptions;
using DocumentManagement.WebApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace DocumentManagement.WebApi.Tests.Controllers
{
    public class DocumentControllerTests
    {
        [Fact]
        public async void ShouldThrowErrorIfNoFileSent()
        {
            // Get a loose automock
            var Mock = AutoMock.GetLoose();

            // Create the Document Contoller
            var documentsController = Mock.Create<DocumentsController>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection());
            var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var context = new ControllerContext(actx);

            documentsController.ControllerContext = context;


            // Invoke the method
            await Assert.ThrowsAsync<CustomException>(() => documentsController.UploadDoument());
        }

        [Fact]
        public async void ShouldThrowErrorIfItNotPdf()
        {
            // Get a loose automock
            var Mock = AutoMock.GetLoose();

            // Create the Document Contoller
            var documentsController = Mock.Create<DocumentsController>();

            var httpContext = new DefaultHttpContext();
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 10, "Data", "dummy.txt");
            file.Headers = new HeaderDictionary();
            file.Headers.Add("Content-Type", "application/text");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
            var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var context = new ControllerContext(actx);

            documentsController.ControllerContext = context;

            // Invoke the method
            await Assert.ThrowsAsync<CustomException>(() => documentsController.UploadDoument());
        }

        [Fact]
        public async void ShouldThrowErrorIfItMaxFileSize()
        {
            // Get a loose automock
            var Mock = AutoMock.GetLoose();

            var someOptions = Options.Create(new AppSettings
            {
                UploadMaxFileSizeInMb = 3
            });

            // Create the Document Contoller
            var documentsController = new DocumentsController(someOptions);

            var httpContext = new DefaultHttpContext();
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 14167898, "Data", "dummy.pdf");
            file.Headers = new HeaderDictionary();
            file.Headers.Add("Content-Type", "application/pdf");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
            var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var context = new ControllerContext(actx);

            documentsController.ControllerContext = context;

            // Invoke the method
            await Assert.ThrowsAsync<CustomException>(() => documentsController.UploadDoument());
        }

        [Fact]
        public async void ShouldUploadFile()
        {
            // Get a loose automock
            var Mock = AutoMock.GetLoose();

            var someOptions = Options.Create(new AppSettings
            {
                UploadMaxFileSizeInMb = 3
            });

            // Create the Document Contoller
            var documentsController = new DocumentsController(someOptions);

            var httpContext = new DefaultHttpContext();
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 15, "Data", "dummy.pdf");
            file.Headers = new HeaderDictionary();
            file.Headers.Add("Content-Type", "application/pdf");
            httpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });
            var actx = new ActionContext(httpContext, new RouteData(), new ControllerActionDescriptor());
            var context = new ControllerContext(actx);

            documentsController.ControllerContext = context;
            
            var mediator = new Mock<IMediator>();
            mediator.Setup(m => m.Send(It.IsAny<UploadFileCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync("filelocation.pdf");

            var servicesMock = new Mock<IServiceProvider>();
            servicesMock.Setup(sp => sp.GetService(typeof(IMediator))).Returns(mediator.Object);
            context.HttpContext.RequestServices = servicesMock.Object;

            // Invoke the method
            await documentsController.UploadDoument();

            mediator.Verify(x => x.Send(It.IsAny<UploadFileCommand>(), It.IsAny<CancellationToken>()));
        }
    }
}
