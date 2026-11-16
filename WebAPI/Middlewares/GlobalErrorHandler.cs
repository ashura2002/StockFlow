using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;
using System.Data;
using WebAPI.Common;

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
            _logger.LogInformation("Exception {Exception}", exception.Message);

            var (statusCode, title, details, errorCode) = exception switch
            {
                ValidationException => (
                StatusCodes.Status400BadRequest,
                "Validation Failed",
                "One or more validation error occurred.",
                ApiErrorCodes.ValidationError),

                DomainRuleViolationException => (
                StatusCodes.Status400BadRequest,
                "Bad Request",
                exception.Message,
                ApiErrorCodes.DomainRuleValidation),

                DBConcurrencyException => (
                StatusCodes.Status409Conflict,
                "Conflict",
                "The resources was modified by another request. Please try again.",
                ApiErrorCodes.ConcurrencyException),

                _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred.",
                ApiErrorCodes.InternalServerError)

            };



            if (statusCode >= 500)
                _logger.LogError(exception, "Unhandled exception occurred.");
            else
                _logger.LogWarning(exception, "Request failed.");



            ProblemDetails problemDetails;

            if(exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(error => error.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(error => error.ErrorMessage)
                        .ToArray());

                problemDetails = new ValidationProblemDetails(errors)
                {
                    Status = statusCode,
                    Title = title,
                    Detail = details,
                    Instance = httpContext.Request.Path
                };

                problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

                problemDetails.Extensions["errorCode"] = errorCode;
            }
            else
            {
                problemDetails = ProblemDetailsFactory.Create(
                httpContext,
                title,
                statusCode,
                details,
                errorCode);
            }

            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/problem+json";


            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }
    }
}