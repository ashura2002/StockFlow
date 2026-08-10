using Application.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;
}
