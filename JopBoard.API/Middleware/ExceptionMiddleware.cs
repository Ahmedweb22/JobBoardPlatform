using System.Net;
using System.Text.Json;

namespace JopBoard.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware>  logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            { 
            _logger.LogError(e , e.Message);
                await HandleExceptionAsync(context , e);
            
        }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception e) 
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = e switch
            { 
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
            _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = e.Message,
                Details = _env.IsDevelopment() ? e.StackTrace?.ToString() : "An internal server error occurred."
            };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}


