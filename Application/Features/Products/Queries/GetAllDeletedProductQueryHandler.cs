using Application.Dtos;
using Application.Interfaces;
using MediatR;

namespace Application.Features.Products.Queries
{
    public sealed class GetAllDeletedProductQueryHandler : IRequestHandler<GetAllDeletedProductQuery, IReadOnlyCollection<DeletedProductResponseDto>>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetAllDeletedProductQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<IReadOnlyCollection<DeletedProductResponseDto>> Handle(GetAllDeletedProductQuery request, CancellationToken cancellationToken)
        {
            return await _productReadRepository.GetAllDeletedProducts(cancellationToken);
        }
    }
}
