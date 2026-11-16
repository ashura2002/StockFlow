using Application.Common.Errors;
using Domain.Enums;

namespace WebAPI.Common
{
    public static class ErrorResponseMapper
    {
        public static (int StatusCode, string Title) Map(Error error)
        {
            return error.ErrorStatus switch
            {
                ErrorStatus.BadRequest => (StatusCodes.Status400BadRequest, "Bad Request"),
                ErrorStatus.Conflict => (StatusCodes.Status409Conflict, "Conflict"),
                ErrorStatus.NotFound => (StatusCodes.Status404NotFound, "Not Found"),
                ErrorStatus.Unauthorized => (StatusCodes.Status401Unauthorized, "Unauthorized"),
                _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
            };
        }
    }
}
