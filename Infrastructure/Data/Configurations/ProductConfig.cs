using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public sealed class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.Stock)
                .IsRequired();

            builder.Property(p => p.ProductName)
                .HasConversion(p => p.Value, p => ProductNameVo.Create(p))
                .HasMaxLength(255)
                .IsRequired();

            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .IsRequired();

            builder.HasOne(p => p.Supplier)
                  .WithMany(s => s.Products)
                  .HasForeignKey(p => p.SupplierId)
                  .IsRequired();

            builder.Property(p => p.ProductDescriptions)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(p => p.ProductImageUrl)
                .IsRequired(false);

            builder.Property(p => p.ProductImagePublicId)
                .IsRequired(false);

            builder.HasIndex(p => p.ProductName)
                .IsUnique();
        }
    }
}
