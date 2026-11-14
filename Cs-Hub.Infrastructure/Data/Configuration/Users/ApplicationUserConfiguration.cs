using Cs_Hub.Domain.entities.Users;
using Cs_Hub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Users
{
    public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("AspNetUsers", t =>
            {
                t.HasCheckConstraint("CK_AspNetUsers_AccountStatus", "[AccountStatus] IN (1, 2, 3)");
            });

            builder.Property(u => u.AccountStatus)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .HasDefaultValue(AccountStatus.Active)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();
        }
    }
}
