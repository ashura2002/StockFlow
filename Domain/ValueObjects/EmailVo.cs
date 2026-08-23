using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public sealed record EmailVo
    {
        public string Value { get;}

        private EmailVo(string value)
        {
            Value = value;
        }

        public static EmailVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) 
                throw new DomainRuleException("Email cannot be empty.");

            value = value.Trim();
            if (!IsValidEmail(value))   
                throw new DomainRuleException("Email invalid format.");

            return new EmailVo(value);
        }

        private static bool IsValidEmail(string email)
        {
            var pattern = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
