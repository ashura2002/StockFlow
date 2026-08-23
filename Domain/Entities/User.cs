using Domain.Enums;
using Domain.Events;
using Domain.Exceptions;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class User : AggregateRoot
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
            var user = new User(email, role, password);
            user.RaiseEvent(new RegisteredUserDomainEvent(user.Id, user.Email.Value));
            return user;
        }


        public void UpdatePassword(PasswordVo newPassword)
        {
            EnsureNotDeleted("Can't update password when user is deleted.");

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

        private void EnsureNotDeleted(string message)
        {
            if (DeletedAt.HasValue)
                throw new DomainRuleException(message);
        }




        // -- PROFILE AGGREGATE CHILD --

        public void CreateProfile(
            FirstNameVo firstname,
            LastNameVo lastname,
            DateOnly dateOfBirth,
            AddressVo address)
        {
            EnsureNotDeleted("Can't create profile if user is deleted.");

            if (Profile != null)
                throw new DomainRuleException("You already had a profile.");

            Profile = Profile.Create(firstname, lastname, dateOfBirth, address);
            Touch();
        }


        public void UpdateProfile(
            FirstNameVo firstName,
            LastNameVo lastName,
            AddressVo address)
        {
            EnsureNotDeleted("Can't update profile if user is deleted.");

            if (Profile == null)
                throw new DomainRuleException("Create your profile first");

            Profile.UpdateFirstName(firstName);
            Profile.UpdateLastName(lastName);
            Profile.UpdateAddress(address);

            Touch();
        }

        public void UpdateProfilePicture(string profilePictureUrl, string profilePicturePublicId)
        {
            if (DeletedAt != null)
                throw new DomainRuleException(
                    "Cannot update profile of a deactivated account.");

            if (Profile is null)
                throw new DomainRuleException("Profile does not exist.");

            Profile.UpdateProfilePicture(profilePictureUrl, profilePicturePublicId);
            Touch();
        }

        public Profile DeleteProfile()
        {
            if (DeletedAt != null)
                throw new DomainRuleException(
                    "Cannot delete profile of a deactivated account.");

            if (Profile is null)
                throw new DomainRuleException("Profile does not exist.");

            // Save the profile before removing it.
            // The Application layer will use it to delete the profile from the database.
            var profile = Profile;

            // Remove the Profile from the aggregate to keep the domain model consistent.
            // This only changes the in-memory object, the database is not affected
            // until the Application layer calls SaveChanges().
            Profile = null;

            Touch();
            return profile;
        }

    }
}
