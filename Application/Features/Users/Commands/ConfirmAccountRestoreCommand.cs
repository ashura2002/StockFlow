using MediatR;


namespace Application.Features.Users.Commands
{
    public sealed record ConfirmAccountRestoreCommand(string Email, string VerificationCode) : IRequest;
}
