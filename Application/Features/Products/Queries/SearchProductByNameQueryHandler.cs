using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed class SearchProductByNameQueryHandler : IRequestHandler<SearchProductByNameQuery, IReadOnlyCollection<ProductResponseDto>>
    {
        private readonly IProductReadRepository _productReadRepository;
        public SearchProductByNameQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<IReadOnlyCollection<ProductResponseDto>> Handle(SearchProductByNameQuery request, CancellationToken cancellationToken)
        {
            return await _productReadRepository.SearchProductsByNameAsync(
                request.ProductName, 
                request.Page, 
                request.PageSize,
                cancellationToken);
        }
    }
}
