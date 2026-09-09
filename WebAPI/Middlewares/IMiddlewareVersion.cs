//using Domain.Exceptions;
//using FluentValidation;

//namespace WebAPI.Middlewares
//{
//    public sealed class IMiddlewareVersion : IMiddleware
//    {
//        private readonly ILogger<IMiddlewareVersion> _logger;

//        public IMiddlewareVersion(
//            ILogger<IMiddlewareVersion> logger)
//        {
//            _logger = logger;
//        }

//        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//        {
//            try
//            {
//                await next(context);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unhandled exception occurred in {RequestPath}.",
//                    context.Request.Path);
//                await HandleException(context, ex);
//            }
//        }


//        private static async Task HandleException(HttpContext context, Exception ex)
//        {
//            // get status code
//            var statusCode = ex switch
//            {
//                DomainNotFoundException => StatusCodes.Status404NotFound,
//                DomainRuleViolationException => StatusCodes.Status400BadRequest,
//                DomainUnauthorizedException => StatusCodes.Status401Unauthorized,
//                DomainConflictException => StatusCodes.Status409Conflict,
//                ValidationException => StatusCodes.Status400BadRequest, // para sa fluent validationnnnnnnn
//                _ => StatusCodes.Status500InternalServerError
//            };

//            context.Response.StatusCode = statusCode;
//            context.Response.ContentType = "application/json";

//            var message = statusCode == StatusCodes.Status500InternalServerError
//                ? "An unexpected error occurred."
//                : ex.Message;

//            var errors = ex is ValidationException validationException ?
//                 validationException.Errors
//                 .GroupBy(error => error.PropertyName)
//                 .ToDictionary(
//                     g => g.Key,
//                     g => g.Select(err => err.ErrorMessage).ToList()) :
//                     null;

//            var response = new ErrorResult(
//                statusCode,
//                message,
//                context.TraceIdentifier,
//                errors);

//            await context.Response.WriteAsJsonAsync(response);
//        }

//        public record ErrorResult(int StatusCode,
//            string Message,
//            string TraceId,
//            object? Errors);
//    }
//}
