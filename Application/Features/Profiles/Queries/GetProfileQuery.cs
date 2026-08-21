
using Application.Dtos;
using MediatR;

namespace Application.Features.Profiles.Queries
{
    public sealed record GetProfileQuery : IRequest<UserWithProfileResponseDto>;
}
