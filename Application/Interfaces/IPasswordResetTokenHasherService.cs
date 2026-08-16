namespace Application.Interfaces
{
    public interface IPasswordResetTokenHasherService
    {
        string Hash(string token);
    }
}
