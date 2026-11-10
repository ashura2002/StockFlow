using Application.Common.Errors;
using Application.Common.Results;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Extensions
{
    public static class ControllerResultExtension
    {
        public static ActionResult<T> ToActionResult<T>(this ControllerBase controller, ResultWithValue<T> result)
        {
            if (result.IsSuccess)
            {
                return controller.Ok(result.Value);
            }

            return controller.ToErrorActionResult(result.Error!);
        }


        public static ActionResult ToActionResult(this ControllerBase controller, Result result)
        {
            if (result.IsSuccess)
                return controller.NoContent();

            return controller.ToErrorActionResult(result.Error!);
        }

        public static ActionResult ToErrorActionResult(this ControllerBase controller, Error error)
        {
            return error.ErrorStatus switch
            {
                ErrorStatus.BadRequest => controller.BadRequest(error),
                ErrorStatus.Conflict => controller.Conflict(error),
                ErrorStatus.NotFound => controller.NotFound(error),
                ErrorStatus.Unauthorized => controller.Unauthorized(error),
                _ => controller.StatusCode(500)
            };
        }
    }
}
