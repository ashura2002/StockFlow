namespace Application.Interfaces
{
    public interface IPasswordService
    {
       string HashPassword(string plainText);
        bool Verify(string password, string hashPassword);
    }
}
