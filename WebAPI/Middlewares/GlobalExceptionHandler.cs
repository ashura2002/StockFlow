using Domain.Exceptions;
using System.Text.Json;

namespace WebAPI.Middlewares
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                _logger.LogInformation("Request started. {RequestPath}", context.Request.Path);
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleException(context, ex);
            }
        }


        private static async Task HandleException(HttpContext context, Exception ex)
        {
            // get status code
            var statusCode = ex switch
            {
                DomainNotFoundException => StatusCodes.Status404NotFound,
                DomainRuleException => StatusCodes.Status400BadRequest,
                DomainUnauthorizedException => StatusCodes.Status401Unauthorized,
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var message = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : ex.Message;

            var response = new ErrorResult(
                statusCode,
                message,
                context.TraceIdentifier);

            await context.Response.WriteAsJsonAsync(response);
        }

        public record ErrorResult(int StatusCode, string Message, string TraceId);
    }
}
