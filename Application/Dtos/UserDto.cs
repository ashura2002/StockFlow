using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dtos
{
    public record UserDto(
        Guid UserId,
        string Email,
        Role Role);
}
