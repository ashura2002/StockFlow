namespace Application.Interfaces
{
    public interface ITokenHasherService
    {
        string Hash(string token);
    }
}
