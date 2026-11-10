
using Domain.Enums;

namespace Application.Common.Errors
{
    public static class UserErrors
    {
        public static readonly Error UserNotFound = new("User.NotFound", "User not found.", ErrorStatus.NotFound);
        public static readonly Error EmailAlreadyInUsed = new("User.EmailAlreadyInUsed", "Email is already in used.", ErrorStatus.Conflict);
        public static readonly Error EmailNotMatch = new("User.NotMatch", "Email does not match.", ErrorStatus.BadRequest);
    }
}
