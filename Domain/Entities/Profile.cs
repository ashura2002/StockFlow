using Domain.Exceptions;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Profile : BaseEntity
    {

        public Guid UserId { get; private set; }
        public User User { get; private set; } = null!;
        public FirstNameVo FirstName { get; private set; }
        public LastNameVo LastName { get; private set; }
        public DateOnly DateOfBirth { get; private set; }
        public string? ProfilePictureUrl { get; private set; }
        public string? ProfilePicturePublicId { get; private set; }
        public AddressVo Address { get; private set; }

        private Profile(
            FirstNameVo firstName,
            LastNameVo lastName,
            DateOnly dateOfBirth,
            AddressVo address,
            string? profilePictureUrl = null,
            string? profilePicturePublicId = null)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Address = address;
            ProfilePictureUrl = profilePictureUrl;
            ProfilePicturePublicId = profilePicturePublicId;
        }

        internal static Profile Create(
            FirstNameVo firstname,
            LastNameVo lastname,
            DateOnly dateOfBirth,
            AddressVo address,
            string? profilePictureUrl = null,
            string? profilePicturePublicId = null)
        {
            if (dateOfBirth > DateOnly.FromDateTime(DateTime.Today))
                throw new DomainBadRequestException("Date of birth cannot be in the future.");

            return new Profile(
                firstname, 
                lastname, 
                dateOfBirth, 
                address,
                profilePictureUrl, 
                profilePicturePublicId);
        }
    }
}
