using Domain.Enums;

namespace Application.Common.Errors
{
    public static class OrderErrors
    {
        public static readonly Error OrderNotFound = new("Order.NotFound", "Order not found", ErrorStatus.NotFound);
        public static readonly Error CancelOwnOrder = new("Order.Unauthorized", "You can only cancel your own order.", ErrorStatus.Unauthorized);
        public static readonly Error ModifyOwnOrder = new("Order.Unauthorized", "You can only modify your own order.", ErrorStatus.Unauthorized);
        public static readonly Error OrderMustContainAtleatOneItem = new("Order.BadRequest", "Order must contain at least one item.", ErrorStatus.BadRequest);
    }
}
