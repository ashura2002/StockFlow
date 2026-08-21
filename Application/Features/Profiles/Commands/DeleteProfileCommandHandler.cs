using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Profiles.Commands
{
    public sealed class DeleteProfileCommandHandler : IRequestHandler<DeleteProfileCommand>
    {
        private readonly IProfileWriteRepository _profileWriteRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageStorage _imageStorage;
        private readonly ILogger<DeleteProfileCommandHandler> _logger;

        public DeleteProfileCommandHandler(
            IProfileWriteRepository profileWriteRepository,
            ICurrentUserService currentUserService,
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork,
            IImageStorage imageStorage,
            ILogger<DeleteProfileCommandHandler> logger)
        {
            _profileWriteRepository = profileWriteRepository;
            _currentUserService = currentUserService;
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
            _imageStorage = imageStorage;
            _logger = logger;
        }


        public async Task Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var user = await _userWriteRepository.GetUserByIdWithProfileAsync(currentUserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found");

            var profile = user.DeleteProfile();
            _profileWriteRepository.Remove(profile);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Delete the image after SaveChanges().
            // If Cloudinary deletion fails, the profile is still deleted,
            // and the orphan image can be cleaned up later.
            if (!string.IsNullOrWhiteSpace(profile.ProfilePicturePublicId))
            {
                try
                {
                    await _imageStorage.DeleteAsync(profile.ProfilePicturePublicId, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "Failed to delete profile picture in Cloudinary {PublicId}",
                        profile.ProfilePicturePublicId);
                }
            }
        }
    }
}
