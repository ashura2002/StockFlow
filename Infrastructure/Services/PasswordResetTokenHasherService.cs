
using Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public sealed class PasswordResetTokenHasherService : IPasswordResetTokenHasherService
    {
        public string Hash(string token)
        {
            // Using a separate hasher instead of BCrypt because reset tokens
            // are already cryptographically random, so SHA-256 is sufficient.

            // Convert token to bytes before hashing.
            // Ex: "abc123" ->  UTF-8 bytes [61, 62, 63, 31, 32, 33].
            var bytes = Encoding.UTF8.GetBytes(token);

            // Generate SHA-256 hash.
            // Ex: "abc123" -> "6ca13d52ca70c883e0f0bb101e425a89..."
            var hash = SHA256.HashData(bytes);

            // Convert hash to a string for database storage.
            return Convert.ToHexString(hash);
        }
    }
}
