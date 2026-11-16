using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Common
{
    public static class ProblemDetailsFactory
    {
        public static ProblemDetails Create(
            HttpContext httpContext,
            string title,
            int statusCode,
            string details,
            string? errorCode = null,
            IDictionary<string, string[]>? errors = null)
        {

            var problemDetails = new ProblemDetails
            {
                Title = title,
                Status = statusCode,
                Detail = details,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["errorCode"] = errorCode;
            problemDetails.Extensions["errors"] = errors;
            return problemDetails;
        }
    }
}
