using Application.Interfaces;
using Domain.Exceptions;
using MediatR;


namespace Application.Features.Orders.Commands
{
    public sealed class UpdateMyOrderCommandHandler : IRequestHandler<UpdateMyOrderCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IOrderWriteRepository _orderWriteRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateMyOrderCommandHandler(
            ICurrentUserService currentUserService,
            IOrderWriteRepository orderWriteRepository,
            IProductWriteRepository productWriteRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _orderWriteRepository = orderWriteRepository;
            _productWriteRepository = productWriteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateMyOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderWriteRepository.GetOrderByIdAsync(request.OrderId, cancellationToken) ??
                throw new DomainNotFoundException("Order not found.");

            if (_currentUserService.UserId != order.UserId)
                throw new DomainUnauthorizedException("You can only modify your own orders.");

            order.EnsureCanBeModified();

            foreach (var item in request.Items)
            {
                var existingItem = order.OrderItems.FirstOrDefault(i => i.ProductId == item.ProductId);

                // first use case add a new item and reserve stock.
                if (existingItem is null)
                {
                    var newProduct = await _productWriteRepository.GetProductByIdAsync(item.ProductId, cancellationToken) ??
                        throw new DomainNotFoundException("Product not found.");
                    newProduct.DecreaseStock(item.Quantity);
                    order.AddItem(item.ProductId, item.Quantity, newProduct.Price);
                    continue;
                }

                // second use case adjust the quantity of an existing item.
                var product = await _productWriteRepository.GetProductByIdAsync(item.ProductId, cancellationToken) ??
                        throw new DomainNotFoundException("Product not found.");

                var oldQuantity = existingItem.Quantity;
                var newQuantity = item.Quantity;
                var difference = newQuantity - oldQuantity;

                if (difference > 0)
                {
                    product.DecreaseStock(difference);
                }
                else if (difference < 0)
                {
                    product.IncreaseStock(oldQuantity - newQuantity);
                }
                existingItem.UpdateQuantity(item.Quantity);
            }


            // use case 3 remove items that are no longer in the request and restore stock.
            // create a snapshot of the current order items because items may be removed
            // from the original collection while iterating.
            var copyOrderItems = order.OrderItems.ToList();
            foreach (var productItem in copyOrderItems)
            {
                var productItemInRequest = request.Items
                    .Any(i => i.ProductId == productItem.ProductId);

                if (productItemInRequest) continue;

                var product = await _productWriteRepository.GetProductByIdAsync(productItem.ProductId, cancellationToken) ??
                                throw new DomainNotFoundException("Product not found");
                product.IncreaseStock(productItem.Quantity);
                order.RemoveItem(productItem.ProductId);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
