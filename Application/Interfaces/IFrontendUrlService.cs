
namespace Application.Interfaces
{
    public interface IFrontendUrlService
    {
        string CreatePasswordResetUrl(string rawToken);
    }
}
