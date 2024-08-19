using System.Net;
namespace Application.Exceptions
{
    public class UnAuthorizedException : AppException
    {
        public UnAuthorizedException(string message)
            : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}
