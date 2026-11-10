
using Domain.Enums;

namespace Application.Common.Errors
{
    public static class ProductErrors
    {
        public static readonly Error ProductNameAlreadyExisted = new("Product.ProductNameAlreadyExisted", "Product name is already existed.", ErrorStatus.Conflict);
        public static readonly Error ProductNotFound = new("Product.NotFound", "Product not found.", ErrorStatus.NotFound);
    }
}
