using System.Net;
using System.Text.Json;

namespace BowlingScoreCalculatorAPI.Middleware
{
    public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex, context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment());
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, bool isDevelopment)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new
            {
                message = "An error occurred while processing your request.",
                details = isDevelopment ? exception.ToString() : "Internal Server Error"
            });
            await response.WriteAsync(result);
        }
    }
}
