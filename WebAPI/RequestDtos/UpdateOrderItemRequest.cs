using Application.Features.OrderItems.Commands;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record UpdateOrderItemRequest
    {
        [Required(ErrorMessage = "Order items required")]
        public required IReadOnlyCollection<CreateOrderItemCommand> OrderItems { get; set; }
    }
}
