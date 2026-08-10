
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Auth.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<string>;
}
