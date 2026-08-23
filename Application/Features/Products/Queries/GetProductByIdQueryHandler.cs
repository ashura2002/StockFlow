using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetProductByIdQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetProductByIdAsync(request.ProductId, cancellationToken)??
                throw new DomainNotFoundException("Product not found");
            return product;
        }
    }
}
