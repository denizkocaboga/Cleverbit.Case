using System;
using System.Net;

namespace Cleverbit.Case.Common.Exceptions
{
    public abstract class HttpResponseException : Exception
    {        
        public abstract HttpStatusCode StatusCode { get; }
        public HttpResponseException() : base() { }
        public HttpResponseException(string message) : base(message) { }
    }
}
