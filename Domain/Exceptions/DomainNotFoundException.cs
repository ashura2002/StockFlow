namespace Domain.Exceptions
{
    public class DomainNotFoundException : Exception
    {
        public DomainNotFoundException(string message):base(message)
        {

        }
    }
}
