using Domain.Enums;

namespace Application.Common.Errors
{
    public sealed record Error(string ErrorCode, string Message, ErrorStatus ErrorStatus);
}
