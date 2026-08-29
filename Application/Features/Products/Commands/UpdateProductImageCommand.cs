using Application.Dtos;
using MediatR;

namespace Application.Features.Products.Commands
{
    public sealed record UpdateProductImageCommand(
        Guid ProductId,
        Stream Stream,
        string FileName,
        string ContentType,
        long FileSize) : IRequest<UploadedImage>;
}
