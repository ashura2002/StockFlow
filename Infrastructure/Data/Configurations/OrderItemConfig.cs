using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Configurations
{
    public sealed class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2) // 18 total digits,  2 digits after decimal Ex. 1500.00, 99.99, 1250000.50
                .IsRequired();

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict) //Prevents deleting product if ordered
                .IsRequired();
        }
    }
}
