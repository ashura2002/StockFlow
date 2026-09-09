using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Middlewares
{
    public sealed class GlobalErrorHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalErrorHandler> _logger;

        public GlobalErrorHandler(
            ILogger<GlobalErrorHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unhandled exception occurred");

            var statusCode = exception switch
            {
                DomainNotFoundException => StatusCodes.Status404NotFound,
                DomainRuleViolationException => StatusCodes.Status400BadRequest,
                DomainUnauthorizedException => StatusCodes.Status401Unauthorized,
                DomainConflictException => StatusCodes.Status409Conflict,
                ValidationException => StatusCodes.Status400BadRequest, // para sa fluent validationnnnnnnn
                _ => StatusCodes.Status500InternalServerError
            };

            var fluentValidationErrors = exception is ValidationException errors
                ? errors.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(err =>
                    err.ErrorMessage).ToList()) : null;


            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Detail = statusCode == StatusCodes.Status500InternalServerError ?
                "An unexpected error occurred."
                : exception.Message
            };

            // only add errors extension when exception came from fluent validation
            if (fluentValidationErrors is not null)
            {
                problemDetails.Extensions["errors"] = fluentValidationErrors;
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
