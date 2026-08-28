namespace Domain.Exceptions
{
    public class DomainBadRequestException:Exception
    {
        public DomainBadRequestException(string message) : base(message) { }
    }
}
