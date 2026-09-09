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
        public string? RestoreCodeHash { get; private set; }
        public DateTime? RestoreCodeExpiresAt { get; private set; }

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
                throw new DomainRuleViolationException("Admin accounts are not allowed to delete their own account.");

            DeletedAt = DateTime.UtcNow;
            Touch();
        }

        public void RestoreAccount()
        {
            if (!DeletedAt.HasValue) return;

            DeletedAt = null;
            Touch();
        }

        private void EnsureNotDeleted(string message)
        {
            if (DeletedAt.HasValue)
                throw new DomainRuleViolationException(message);
        }

        public void SetRestoreCode(string codeHash, DateTime expiresAt)
        {
            if (!DeletedAt.HasValue)
                throw new DomainRuleViolationException("Only deleted accounts can request account restoration.");

            RestoreCodeHash = codeHash;
            RestoreCodeExpiresAt = expiresAt;
            Touch();
        }

        public void VerifyRestoreCode(string codeHash)
        {
            if (RestoreCodeHash is null || RestoreCodeExpiresAt is null)
                throw new DomainRuleViolationException("No restoration code exists.");

            if (RestoreCodeExpiresAt <= DateTime.UtcNow)
                throw new DomainRuleViolationException("Restoration code has expired.");

            if (RestoreCodeHash != codeHash)
                throw new DomainRuleViolationException("Invalid restoration code.");

            RestoreCodeHash = null;
            RestoreCodeExpiresAt = null;
            Touch();
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
                throw new DomainRuleViolationException("You already had a profile.");

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
                throw new DomainRuleViolationException("Create your profile first");

            Profile.UpdateFirstName(firstName);
            Profile.UpdateLastName(lastName);
            Profile.UpdateAddress(address);

            Touch();
        }

        public void UpdateProfilePicture(string profilePictureUrl, string profilePicturePublicId)
        {
            EnsureNotDeleted("Cannot update profile of a deactivated account.");

            if (Profile is null)
                throw new DomainRuleViolationException("Profile does not exist.");

            Profile.UpdateProfilePicture(profilePictureUrl, profilePicturePublicId);
            Touch();
        }

        public void DeleteProfile()
        {
            EnsureNotDeleted("Cannot delete profile of a deactivated account.");

            if (Profile is null)
                throw new DomainRuleViolationException("Profile does not exist.");

            Profile = null;
            Touch();
        }

    }
}
