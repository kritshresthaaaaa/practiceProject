namespace WebHost.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomMiddleware> _logger;
        public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {

            _logger.LogInformation("Custom Middleware executing..");
            await _next(context);
            _logger.LogInformation("Custom Middleware executed..");
            // Do something after the next middleware
        }

    }
}
