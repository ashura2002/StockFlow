using Application.Common.Results;
using Application.Dtos;
using MediatR;

namespace Application.Features.Profiles.Commands
{
    public record UpdateProfilePictureCommand(
        Stream Stream,
        string FileName,
        string ContentType,
        long FileSize) : IRequest<ResultWithValue<UploadedImage>>;
}