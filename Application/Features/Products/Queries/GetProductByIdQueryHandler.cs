using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly ICacheService _cacheService;

        public GetProductByIdQueryHandler(
            IProductReadRepository productReadRepository,
            ICacheService cacheService)
        {
            _productReadRepository = productReadRepository;
            _cacheService = cacheService;
        }

        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // set cache key first
            var cacheKey = $"product:{request.ProductId}";

            return await _cacheService.GetOrCreateAsync(
                cacheKey,
                () => _productReadRepository.GetProductByIdAsync(request.ProductId, cancellationToken),
                TimeSpan.FromMinutes(5)) ??
                throw new DomainNotFoundException("Product not found.");
        }
    }
}
