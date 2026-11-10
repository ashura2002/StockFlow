using Application.Common.Results;
using MediatR;

namespace Application.Features.Profiles.Commands
{
    public sealed record DeleteProfileCommand : IRequest<Result>;
}
