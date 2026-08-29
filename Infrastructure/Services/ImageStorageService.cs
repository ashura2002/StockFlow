using Application.Dtos;
using Application.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Infrastructure.Services
{
    public sealed class ImageStorageService : IImageStorage
    {
        private readonly Cloudinary _cloudinary;

        public ImageStorageService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task DeleteAsync(string publicUrl, CancellationToken cancellationToken)
        {
            var deleteParams = new DeletionParams(publicUrl);

            var result = await _cloudinary.DestroyAsync(deleteParams);
            if (result.Error is not null)
                throw new InvalidOperationException(result.Error.Message);
        }

        public async Task<UploadedImage> UploadPictureAsync(Stream stream, string fileName, CancellationToken cancellationToken)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
                Folder = "StockFlow/images",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false
            };

            var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);
            if (result.Error is not null)
                throw new InvalidOperationException(result.Error.Message);

            return new UploadedImage(
            result.SecureUrl.ToString(),
            result.PublicId);
        }
    }
}
