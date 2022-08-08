using System.Net;

namespace Cleverbit.Case.Common.Exceptions
{
    public class UnprocessableException : HttpResponseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.UnprocessableEntity;
        public UnprocessableException() : base() { }
        public UnprocessableException(string message) : base(message) { }
    }
}
