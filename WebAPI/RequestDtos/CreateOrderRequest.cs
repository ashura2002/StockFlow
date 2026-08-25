using Application.Features.OrderItems.Commands;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.RequestDtos
{
    public sealed record CreateOrderRequest
    {
        [Required(ErrorMessage = "Order items required")]
        public required IReadOnlyCollection<CreateOrderItemCommand> OrderItems { get; set; }
    }
}
