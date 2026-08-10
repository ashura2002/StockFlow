using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Supplier : BaseEntity
    {
        public string Name { get; private set; }
        public EmailVo Email { get; private set; }
        public PhoneNumberVo PhoneNumber { get; private set; }
        public AddressVo Address { get; private set; }

        private readonly List<Product> _products = new();
        public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

        private Supplier(
            string name,
            EmailVo email,
            PhoneNumberVo phoneNumber,
            AddressVo address)
        {
            Name = name;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
        }
    }
}
