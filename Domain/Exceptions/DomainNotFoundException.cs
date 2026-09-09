namespace Domain.Exceptions
{
    public sealed class DomainNotFoundException : Exception
    {
        public DomainNotFoundException(string message):base(message)
        {

        }
    }
}
