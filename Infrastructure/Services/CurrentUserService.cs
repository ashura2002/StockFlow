using Application.Interfaces;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException("UserId claim is missing");

                // convert string to Guid
                return Guid.Parse(userId);
            }
        }

        public Role Role
        {
            get
            {
                var userRole = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(userRole)) throw new UnauthorizedAccessException("User Role is missing");


                // convert string to Enums
                return Enum.Parse<Role>(userRole);
            }
        }
    }
}
