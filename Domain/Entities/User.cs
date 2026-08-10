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


        public void UpdateEmail(EmailVo newEmail)
        {
            if (Email == newEmail) return;
            Email = newEmail;
            Touch();
        }

        public void UpdatePassword(PasswordVo newPassword)
        {
            if (Password == newPassword) return;
            Password = newPassword;
            Touch();
        }



        // updating profile
        public void CreateProfile(
            FirstNameVo firstname,
            LastNameVo lastname,
            DateOnly dateOfBirth,
            AddressVo address)
        {
            if (DeletedAt.HasValue)
                throw new DomainBadRequestException("Cannot create profile for soft deleted user.");

            if (Profile != null)
                throw new DomainBadRequestException("You already had a profile.");

            Profile = Profile.Create(firstname, lastname, dateOfBirth, address);
            Touch();
        }
    }
}
