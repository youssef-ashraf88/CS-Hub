using Cs_Hub.Domain.entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Users
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("UserProfiles", t =>
            {
                t.HasCheckConstraint("CK_UserProfiles_DisplayName_NotBlank", "LEN(LTRIM(RTRIM([DisplayName]))) > 0");
            });

            builder.HasKey(p => p.UserId);
            builder.Property(p => p.UserId).ValueGeneratedNever();

            builder.Property(p => p.DisplayName).IsRequired().HasMaxLength(80);
            builder.Property(p => p.HeadLine).HasMaxLength(120);
            builder.Property(p => p.Bio).HasMaxLength(500);
            builder.Property(p => p.Location).HasMaxLength(100);
            builder.Property(p => p.WebsiteUrl).HasMaxLength(2048);
            builder.Property(p => p.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();
            builder.Property(p => p.UpdatedAt)
                .HasColumnType("datetime2(3)");

            builder.HasOne(p => p.User)
                .WithOne(u => u.UserProfile)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.AvatarAttachment)
                .WithOne()
                .HasForeignKey<UserProfile>(p => p.AvatarAttachmentId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasIndex(p => p.AvatarAttachmentId)
                .IsUnique()
                .HasFilter("[AvatarAttachmentId] IS NOT NULL")
                .HasDatabaseName("UX_UserProfiles_AvatarAttachmentId");
        }
    }
}
