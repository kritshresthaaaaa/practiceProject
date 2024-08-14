using Newtonsoft.Json;
using System.Net;
using WebHost.Exceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;  // RequestDelegate is a delegate that represents the next middleware in the pipeline.
    //requestdelegate is injected? 

    private readonly ILogger<ExceptionHandlingMiddleware> _logger; 

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    // InvokeAsync is a method that is called by the runtime. It is used to handle the exception and log the error.
    public async Task InvokeAsync(HttpContext context) // HttpContext is a class that encapsulates all HTTP-specific information about an individual HTTP request.
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
        string result;

        if (exception is AppException appException)
        {
            statusCode = appException.StatusCode;

            result = JsonConvert.SerializeObject(new { error = appException.Message, code = appException.ErrorCode });
        }
        else
        {
            result = JsonConvert.SerializeObject(new { error = "An unexpected error occurred." });
        }

        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }
/*    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode statusCode;
        string result;

        switch (exception)
        {
            case AppException appException:
                statusCode = appException.StatusCode;
                result = JsonConvert.SerializeObject(new { error = appException.Message, code = appException.ErrorCode });
                break;

            case KeyNotFoundException keyNotFoundException:
                statusCode = HttpStatusCode.NotFound;
                result = JsonConvert.SerializeObject(new { error = keyNotFoundException.Message });
                break;

            case UnauthorizedAccessException unauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                result = JsonConvert.SerializeObject(new { error = unauthorizedAccessException.Message });
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                result = JsonConvert.SerializeObject(new { error = "An unexpected error occurred." });
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        return context.Response.WriteAsync(result);
    }*/

}
