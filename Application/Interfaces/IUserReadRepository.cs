using Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IUserReadRepository
    {
        Task<IReadOnlyCollection<UserDto>> GetAllUsersAsync(int page, int pageSize, CancellationToken cancellationToken);
        Task<bool> IsEmailExistAsync(string email, CancellationToken cancellationToken);
    }
}
