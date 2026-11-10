using Application.Common.Errors;
using Application.Common.Results;
using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ResultWithValue<ProductResponseDto>>
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

        public async Task<ResultWithValue<ProductResponseDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            // set cache key first
            var cacheKey = $"product:{request.ProductId}";

            var product = await _productReadRepository.GetProductByIdAsync(request.ProductId, cancellationToken);
            if (product is null)
                return ResultWithValue<ProductResponseDto>.Failure(ProductErrors.ProductNotFound);

            _cacheService.Set(cacheKey, product, TimeSpan.FromMinutes(5));

            return ResultWithValue<ProductResponseDto>.Success(product);
        }
    }
}
