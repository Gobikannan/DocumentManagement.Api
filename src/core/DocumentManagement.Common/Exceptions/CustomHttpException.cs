using System;
using System.Net;

namespace DocumentManagement.Common.Exceptions
{
    public class CustomException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public CustomException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public CustomException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
