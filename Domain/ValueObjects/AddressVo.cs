using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public record AddressVo
    {
        public string Value { get; }

        private AddressVo(string value)
        {
            Value = value;
        }

        public static AddressVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainRuleViolationException("Address cannot be empty.");
            value = value.Trim();
            if (value.Length < 5)
                throw new DomainRuleViolationException("Address must be at least 5 characters.");
            return new AddressVo(value);
        }
    }
}