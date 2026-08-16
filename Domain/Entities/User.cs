using Domain.Enums;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public EmailVo Email { get; private set; }
        public Role Role { get; private set; }
        public PasswordVo Password { get; private set; }
        public Profile? Profile { get; private set; }
        public DateTime? DeletedAt { get; private set; }

        private User(
            EmailVo email,
            Role role,
            PasswordVo password)
        {
            Email = email;
            Role = role;
            Password = password;
        }

        public static User Create(
            EmailVo email, 
            Role role, 
            PasswordVo password)
        {
            return new User(email, role, password);
        }


        public void UpdatePassword(PasswordVo newPassword)
        {
            EnsureNotDeleted();

            if (Password == newPassword) return;

            Password = newPassword;
            Touch();
        }

        public void SoftDelete()
        {
            if (DeletedAt.HasValue) return;
            if (Role == Role.Admin)
                throw new DomainRuleException("Admin accounts are not allowed to delete their own account.");

            DeletedAt = DateTime.UtcNow;
            Touch();
        }

        private void EnsureNotDeleted()
        {
            if (DeletedAt.HasValue)
                throw new DomainRuleException("Cannot create profile for soft deleted user.");
        }




        // -- PROFILE AGGREGATE CHILD --

        public void CreateProfile(
            FirstNameVo firstname,
            LastNameVo lastname,
            DateOnly dateOfBirth,
            AddressVo address)
        {
            EnsureNotDeleted();

            if (Profile != null)
                throw new DomainRuleException("You already had a profile.");

            Profile = Profile.Create(firstname, lastname, dateOfBirth, address);
            Touch();
        }
    }
}
