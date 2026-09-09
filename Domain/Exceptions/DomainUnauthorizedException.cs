
namespace Domain.Exceptions
{
    public sealed class DomainUnauthorizedException:Exception
    {
        public DomainUnauthorizedException(string message):base(message)
        {
            
        }
    }
}
