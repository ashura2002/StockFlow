using Application.Interfaces;

namespace Infrastructure.Services
{
    internal class PasswordService : IPasswordService
    {
        public string HashPassword(string plainText)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainText);
        }

        public bool Verify(string password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashPassword);
        }
    }
}
