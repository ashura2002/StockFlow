using Domain.ValueObjects;


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

        public static Supplier Create(
            string name,
            EmailVo email,
            PhoneNumberVo phoneNumber,
            AddressVo address)
        {
            return new Supplier(name,email,phoneNumber,address);
        }

        public void UpdateSuplierName(string newName)
        {
            if (Name == newName) return;

            Name = newName;
            Touch();
        }

        public void UpdateSupplierEmail(EmailVo newEmail)
        {
            if (Email == newEmail) return;

            Email = newEmail;
            Touch();
        }

        public void UpdateSupplierPhoneNumber(PhoneNumberVo newPhoneNumber)
        {
            if (PhoneNumber == newPhoneNumber) return;

            PhoneNumber = newPhoneNumber;
            Touch();
        }

        public void UpdateSupplierAddress(AddressVo newAddress)
        {
            if (Address == newAddress) return;

            Address = newAddress;
            Touch();
        }
    }
}
