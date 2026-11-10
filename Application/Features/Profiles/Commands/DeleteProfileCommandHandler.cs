using Application.Common.Errors;
using Application.Common.Results;
using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Profiles.Commands
{
    public sealed class DeleteProfileCommandHandler : IRequestHandler<DeleteProfileCommand, Result>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageStorage _imageStorage;
        private readonly ILogger<DeleteProfileCommandHandler> _logger;

        public DeleteProfileCommandHandler(
            ICurrentUserService currentUserService,
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork,
            IImageStorage imageStorage,
            ILogger<DeleteProfileCommandHandler> logger)
        {
            _currentUserService = currentUserService;
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
            _imageStorage = imageStorage;
            _logger = logger;
        }


        public async Task<Result> Handle(DeleteProfileCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;
            var user = await _userWriteRepository.GetUserByIdWithProfileAsync(currentUserId, cancellationToken);
            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);

            user.DeleteProfile();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Delete the image after SaveChanges().
            // If Cloudinary deletion fails, the profile is still deleted,
            // and the orphan image can be cleaned up later.
            if (!string.IsNullOrWhiteSpace(user.Profile?.ProfilePicturePublicId))
            {
                try
                {
                    await _imageStorage.DeleteAsync(user.Profile.ProfilePicturePublicId, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex,
                        "Failed to delete profile picture in Cloudinary {PublicId}",
                        user.Profile.ProfilePicturePublicId);
                }
            }

            return Result.Success();
        }
    }
}
