using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Data.Configurations
{
    public sealed class ProfileConfig : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .HasConversion(u => u.Value, u => FirstNameVo.Create(u))
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.LastName)
               .HasConversion(u => u.Value, u => LastNameVo.Create(u))
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(p => p.Address)
               .HasConversion(u => u.Value, u => AddressVo.Create(u))
               .HasMaxLength(255)
               .IsRequired();

            // relation
            builder.HasOne(p => p.User)
                .WithOne(u => u.Profile)
                .HasForeignKey<Profile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasQueryFilter(p => p.User.DeletedAt == null);
        }
    }
}
