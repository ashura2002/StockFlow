using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed class GetAllProductQueryHandler : IRequestHandler<GetAllProductsQuery, IReadOnlyCollection<ProductResponseDto>>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetAllProductQueryHandler(
            IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<IReadOnlyCollection<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _productReadRepository.GetAllProductsAsync(request.Page, request.PageSize, cancellationToken);
        }
    }
}
