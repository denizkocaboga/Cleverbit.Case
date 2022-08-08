using System;
using System.Net;

namespace Cleverbit.Case.Common.Exceptions
{
    public class NotFoundException : HttpResponseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public NotFoundException() : base() { }
        public NotFoundException(string message) : base(message) { }
    }
}
