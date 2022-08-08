using System.Net;

namespace Cleverbit.Case.Common.Exceptions
{

    public class ConflictException : HttpResponseException
    {
        public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;
        public ConflictException() : base() { }
        public ConflictException(string message) : base(message) { }

        public ConflictException(int id) : this($"id ({id}) already exists") { }


    }
}
