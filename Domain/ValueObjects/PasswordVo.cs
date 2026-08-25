using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

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
                throw new DomainRuleException("Password cannot be empty.");
            
            value = value.Trim();

            if (value.Length < 5)
                throw new DomainRuleException("Invalid password, Password must contain 5 or more characters");

            return new PasswordVo(value);
        }
    }
}
