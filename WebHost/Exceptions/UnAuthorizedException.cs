using System.Net;
namespace WebHost.Exceptions
{
    public class UnAuthorizedException : AppException
    {
        public UnAuthorizedException(string message)
            : base(message, HttpStatusCode.Unauthorized)
        {
        }
    }
}
