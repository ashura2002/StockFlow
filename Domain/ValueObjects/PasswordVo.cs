using Domain.Exceptions;

namespace Domain.ValueObjects
{
    public class PasswordVo
    {
        public string Value { get; }

        private PasswordVo(string value)
        {
            Value = value;
        }

        public static PasswordVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainRuleViolationException("Password cannot be empty.");
            
            value = value.Trim();

            if (value.Length < 8)
                throw new DomainRuleViolationException("Invalid password, Password must contain 8 or more characters");

            return new PasswordVo(value);
        }
    }
}
