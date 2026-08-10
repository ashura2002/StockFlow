using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data.Configurations
{
    public class SupplierConfig : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(s => s.Email)
                .HasConversion(s => s.Value, s => EmailVo.Create(s))
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(s => s.PhoneNumber)
               .HasConversion(s => s.Value, s => PhoneNumberVo.Create(s))
               .HasMaxLength(255)
               .IsRequired();

            builder.Property(s => s.Address)
               .HasConversion(s => s.Value, s => AddressVo.Create(s))
               .HasMaxLength(255)
               .IsRequired();

        }
    }
}
