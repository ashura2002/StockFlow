using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class AddressVo
    {
        public string Value { get; }

        private AddressVo(string value)
        {
            Value = value;
        }

        public static AddressVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainRuleException("Address cannot be empty.");
            value = value.Trim();
            if (value.Length < 5)
                throw new DomainRuleException("Address must be at least 5 characters.");
            return new AddressVo(value);
        }
    }
}