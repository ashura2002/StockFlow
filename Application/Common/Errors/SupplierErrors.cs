
using Domain.Enums;

namespace Application.Common.Errors
{
    public static class SupplierErrors
    {
        public static readonly Error SuplierEmailConflict = new("SupplierEmail.Conflict", "Supplier's email is already exist.", ErrorStatus.Conflict);
        public static readonly Error SupplierNotFound = new("Supplier.NotFound", "Supplier not found.", ErrorStatus.NotFound);
    }
}
