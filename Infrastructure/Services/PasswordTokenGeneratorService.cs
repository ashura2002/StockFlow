using Application.Interfaces;
using System.Security.Cryptography;

namespace Infrastructure.Services
{
    public sealed class PasswordTokenGeneratorService : IPasswordTokenGeneratorService
    {
        public string Generate()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(32);
            // transform them into URL safe characters
            // get something like k3J8mF5Qx_9zL2...
            // instead of k3J8mF5Qx/9zL2==...
            return Convert.ToBase64String(tokenBytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}
