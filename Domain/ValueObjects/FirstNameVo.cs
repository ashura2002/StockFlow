using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ValueObjects
{
    public class FirstNameVo
    {
        public string Value { get; }

        private FirstNameVo(string value)
        {
            Value = value;
        }

        public static FirstNameVo Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainBadRequestException("Firstname cannot be empty.");
            value = value.Trim();
            value = char.ToUpper(value[0]) + value.Substring(1).ToLower();

            if (value.Length <= 3)
                throw new DomainBadRequestException("Invalid value, Firstname must above 3 characters length.");

            return new FirstNameVo(value);
        }
    }
}
