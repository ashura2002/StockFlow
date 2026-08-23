using Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public sealed record PhoneNumberVo
    {
        public string Value { get; }

        private PhoneNumberVo(string value)
        {
            Value = value;
        }

        public static PhoneNumberVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainRuleException(
                    "Phone number cannot be empty.");

            value = value.Trim();

            if (!Regex.IsMatch(value, @"^09\d{9}$"))
                throw new DomainRuleException(
                    "Invalid Philippine mobile phone number.");

            return new PhoneNumberVo(value);
        }
    }
}