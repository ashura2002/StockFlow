using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Configurations
{
    public sealed class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CategoryName)
                .HasConversion(c => c.Value, c => CategoryNameVo.Create(c))
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.HasIndex(c => c.CategoryName)
                .IsUnique();
        }
    }
}
