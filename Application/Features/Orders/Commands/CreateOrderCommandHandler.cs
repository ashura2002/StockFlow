using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Features.Orders.Commands
{
    public sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            ICurrentUserService currentUserService,
            IProductWriteRepository productWriteRepository,
            IOrderWriteRepository orderWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _productWriteRepository = productWriteRepository;
            _orderWriteRepository = orderWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.UserId;

            // Validation an order must contain at least one item
            if (request.Items.Count == 0)
                throw new DomainRuleException("Order must contain at least one item.");

            var order = Order.Create(currentUserId); // create the Order aggregate

            // loop plus add items
            foreach (var item in request.Items)
            {
                // load the product needer for the order
                var product = await _productWriteRepository.GetProductByIdAsync(item.ProductId, cancellationToken) ??
                    throw new DomainNotFoundException("Product not found.");

                // Order manages its own OrderItems
                order.AddItem(product.Id, item.Quantity, product.Price);
                // Product manages its own stock
                product.DecreaseStock(item.Quantity);
            }
                
            _orderWriteRepository.Add(order);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return order.Id;
        }
    }
}
