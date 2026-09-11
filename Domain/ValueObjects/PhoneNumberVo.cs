using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public record PhoneNumberVo
    {
        public string Value { get; }

        private PhoneNumberVo(string value)
        {
            Value = value;
        }

        public static PhoneNumberVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainRuleViolationException(
                    "Phone number cannot be empty.");

            value = value.Trim();

            if (!Regex.IsMatch(value, @"^09\d{9}$"))
                throw new DomainRuleViolationException(
                    "Invalid Philippine mobile phone number.");

            return new PhoneNumberVo(value);
        }
    }
}