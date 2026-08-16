using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class LastNameVo
    {
        public string Value { get; }

        private LastNameVo(string value)
        {
            Value = value;
        }

        public static LastNameVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainRuleException("Lastname cannot be empty.");
            value = value.Trim();
            value = char.ToUpper(value[0]) + value.Substring(1).ToLower();

            if (value.Length <= 3)
                throw new DomainRuleException("Invalid value, Lastname must above 3 characters length.");

            return new LastNameVo(value);
        }
    }
}
