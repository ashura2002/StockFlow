using Domain.Enums;

namespace Application.Common.Errors
{
    public static class AuthErrors
    {
        public static readonly Error InvalidCredentials = new(
            "Auth.InvalidCredentials", 
            "Email or password is incorrect",
            ErrorStatus.Unauthorized);

        public static readonly Error InvalidPasswordResetToken = new(
            "Auth.InvalidPasswordResetToken",
            "Invalid or expired password reset token.",
            ErrorStatus.BadRequest);

        public static readonly Error RefreshTokenInvalid = new(
        "Auth.InvalidRefreshToken",
        "Invalid refresh token.",
        ErrorStatus.Unauthorized);

        public static readonly Error IncorrectPassword = new("User.IncorrectPassword", "Incorrect Password", ErrorStatus.BadRequest);
    }
}
