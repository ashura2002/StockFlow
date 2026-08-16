using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Configurations
{
    public sealed class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .HasConversion(u => u.Value, u => EmailVo.Create(u))
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(u => u.Password)
                .HasConversion(u => u.Value, u => PasswordVo.Create(u))
                .HasMaxLength(255)
                .IsRequired();

            // for constraint unique values
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // global filter for soft deleted user
            builder.HasQueryFilter(u => u.DeletedAt == null);
        }
    }
}
