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
                throw new DomainBadRequestException("Admin accounts are not allowed to delete their own account.");

            DeletedAt = DateTime.UtcNow;
            Touch();
        }

        private void EnsureNotDeleted(string message)
        {
            if (DeletedAt.HasValue)
                throw new DomainBadRequestException(message);
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
                throw new DomainBadRequestException("You already had a profile.");

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
                throw new DomainBadRequestException("Create your profile first");

            Profile.UpdateFirstName(firstName);
            Profile.UpdateLastName(lastName);
            Profile.UpdateAddress(address);

            Touch();
        }

        public void UpdateProfilePicture(string profilePictureUrl, string profilePicturePublicId)
        {
            EnsureNotDeleted("Cannot update profile of a deactivated account.");

            if (Profile is null)
                throw new DomainBadRequestException("Profile does not exist.");

            Profile.UpdateProfilePicture(profilePictureUrl, profilePicturePublicId);
            Touch();
        }

        public void DeleteProfile()
        {
            EnsureNotDeleted("Cannot delete profile of a deactivated account.");

            if (Profile is null)
                throw new DomainBadRequestException("Profile does not exist.");

            Profile = null;
            Touch();
        }

    }
}
