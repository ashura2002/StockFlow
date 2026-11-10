using Application.Common.Errors;
using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Features.Products.Commands
{
    public sealed class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommand, ResultWithValue<UploadedImage>>
    {
        private readonly IImageStorage _imageStorage;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly ILogger<UpdateProductImageCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductImageCommandHandler(
            IImageStorage imageStorage,
            IProductWriteRepository productWriteRepository,
            ILogger<UpdateProductImageCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _imageStorage = imageStorage;
            _productWriteRepository = productWriteRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResultWithValue<UploadedImage>> Handle(UpdateProductImageCommand request, CancellationToken cancellationToken)
        {
            ValidateFile(request);

            var product = await _productWriteRepository.GetProductByIdAsync(request.ProductId, cancellationToken);
            if (product is null)
                return ResultWithValue<UploadedImage>.Failure(ProductErrors.ProductNotFound);

            var oldProductImagePublicId = product.ProductImagePublicId;

            UploadedImage newProductImage = await _imageStorage.UploadPictureAsync(
                request.Stream,
                request.FileName,
                cancellationToken);
            try
            {
                product.UpdateProductImage(newProductImage.Url, newProductImage.PublicId);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await RollbackUploadedImageAsync(newProductImage.PublicId, ex, cancellationToken);
                throw;
            }


            await DeleteOldProductPictureAsync(oldProductImagePublicId, cancellationToken);
            return ResultWithValue<UploadedImage>.Success(newProductImage);
        }
        // helpers
        private async Task RollbackUploadedImageAsync(string publicId, Exception exception, CancellationToken cancellationToken)
        {
            try
            {
                await _imageStorage.DeleteAsync(publicId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to rollback uploaded image {imageurl}", publicId);
            }
        }

        private async Task DeleteOldProductPictureAsync(string? oldProductPicturePublicId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(oldProductPicturePublicId)) return;

            try
            {
                await _imageStorage.DeleteAsync(oldProductPicturePublicId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete old image {imageUrl}", oldProductPicturePublicId);
            }
        }

        private static void ValidateFile(UpdateProductImageCommand request)
        {
            const long maxFileSize = 5 * 1024 * 1024;
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };

            if (request.Stream is null || request.FileSize == 0)
                throw new DomainRuleViolationException("Please select an image.");

            if (request.FileSize > maxFileSize)
                throw new DomainRuleViolationException("Image size cannot exceed 5 MB.");

            if (!allowedTypes.Contains(request.ContentType))
                throw new DomainRuleViolationException("Only JPEG, PNG, and WEBP images are allowed.");
        }
    }
}
