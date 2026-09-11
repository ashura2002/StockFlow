using Domain.Exceptions;


namespace Domain.ValueObjects
{
    public record LastNameVo
    {
        public string Value { get; }

        private LastNameVo(string value)
        {
            Value = value;
        }

        public static LastNameVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainRuleViolationException("Lastname cannot be empty.");
            value = value.Trim();
            value = char.ToUpper(value[0]) + value.Substring(1).ToLower();

            if (value.Length <= 3)
                throw new DomainRuleViolationException("Invalid value, Lastname must above 3 characters length.");

            return new LastNameVo(value);
        }
    }
}
