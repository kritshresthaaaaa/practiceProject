namespace WebHost.Exceptions
{
    public class UnAuthorizedException : AppException
    {
        public UnAuthorizedException(string message)
            : base(message, System.Net.HttpStatusCode.Unauthorized)
        {
        }
    }
}
