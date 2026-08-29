using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Profiles.Commands
{
    public sealed class UpdateProfilePictureCommandHandler : IRequestHandler<UpdateProfilePictureCommand, UploadedImage>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserWriteRepository _userWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateProfilePictureCommandHandler> _logger;
        private readonly IImageStorage _imageStorage;

        public UpdateProfilePictureCommandHandler(
            ICurrentUserService currentUserService,
            IUserWriteRepository userWriteRepository,
            IUnitOfWork unitOfWork,
            ILogger<UpdateProfilePictureCommandHandler> logger,
            IImageStorage imageStorage)
        {
            _currentUserService = currentUserService;
            _userWriteRepository = userWriteRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _imageStorage = imageStorage;
        }

        public async Task<UploadedImage> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            ValidateFile(request);

            var currentUserId = _currentUserService.UserId;
            var user = await _userWriteRepository.GetUserByIdWithProfileAsync(currentUserId, cancellationToken) ??
                throw new DomainNotFoundException("User not found");

            // for existed profile picture public id
            string? oldProfilePicturePublicId = user.Profile?.ProfilePicturePublicId;

            UploadedImage newProfilePicture = await _imageStorage.UploadPictureAsync(
              request.Stream,
              request.FileName,
              cancellationToken);

            try
            {
                user.UpdateProfilePicture(newProfilePicture.Url, newProfilePicture.PublicId);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await RollbackUploadedImageAsync(newProfilePicture.PublicId, ex, cancellationToken);
                throw;
            }


            await DeleteOldProfilePictureAsync(oldProfilePicturePublicId, cancellationToken);

            return newProfilePicture;
        }

        // helpers
        private async Task RollbackUploadedImageAsync(string publicId, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Failed to update profile picture for user {UserId}",
                _currentUserService.UserId);

            try
            {
                await _imageStorage.DeleteAsync(publicId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to rollback uploaded image {imageurl}", publicId);
            }
        }

        private async Task DeleteOldProfilePictureAsync(string? oldProfilePicturePublicId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(oldProfilePicturePublicId)) return;

            try
            {
                await _imageStorage.DeleteAsync(oldProfilePicturePublicId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete old image {imageUrl}", oldProfilePicturePublicId);
            }
        }

        private static void ValidateFile(UpdateProfilePictureCommand request)
        {
            const long maxFileSize = 5 * 1024 * 1024;
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };

            if (request.Stream is null || request.FileSize == 0)
                throw new DomainBadRequestException("Please select an image.");

            if (request.FileSize > maxFileSize)
                throw new DomainBadRequestException("Image size cannot exceed 5 MB.");

            if (!allowedTypes.Contains(request.ContentType))
                throw new DomainBadRequestException("Only JPEG, PNG, and WEBP images are allowed.");
        }
    }
}
