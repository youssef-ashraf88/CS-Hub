using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Content
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts", t =>
            {
                t.HasCheckConstraint("CK_Posts_ModerationStatus", "[ModerationStatus] IN (0, 1, 2, 3, 4)");
                t.HasCheckConstraint("CK_Posts_Approved_Has_PublishedAt", "[ModerationStatus] <> 1 OR [PublishedAt] IS NOT NULL");
                t.HasCheckConstraint("CK_Posts_Body_NotBlank", "LEN(LTRIM(RTRIM([Body]))) > 0");
            });

            builder.HasKey(p => p.PostId);
            builder.Property(p => p.PostId).ValueGeneratedNever();
            builder.Property(p => p.AuthorId).IsRequired();
            builder.Property(p => p.Title).HasMaxLength(200);
            builder.Property(p => p.Body).IsRequired().HasMaxLength(5000);
            builder.Property(p => p.ModerationStatus)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .HasDefaultValue(ModerationStatus.Pending)
                .IsRequired();
            builder.Property(p => p.ModerationAttempts)
                .HasColumnType("tinyint")
                .HasDefaultValue((byte)0)
                .IsRequired();
            builder.Property(p => p.PublishedAt).HasColumnType("datetime2(3)");
            builder.Property(p => p.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();
            builder.Property(p => p.UpdatedAt).HasColumnType("datetime2(3)");
            builder.Property(p => p.DeletedAt).HasColumnType("datetime2(3)");

            builder.HasQueryFilter(p => p.DeletedAt == null);

            builder.HasIndex(p => new { p.PublishedAt, p.PostId })
                .IsDescending(true, true)
                .IncludeProperties(p => p.AuthorId)
                .HasFilter("[ModerationStatus] = 1 AND [DeletedAt] IS NULL")
                .HasDatabaseName("IX_Posts_Public_Feed");

            builder.HasIndex(p => new { p.AuthorId, p.CreatedAt, p.PostId })
                .IsDescending(false, true, true)
                .HasDatabaseName("IX_Posts_Author_Created");

            builder.HasIndex(p => new { p.ModerationStatus, p.CreatedAt })
                .HasFilter("[ModerationStatus] IN (0, 3)")
                .HasDatabaseName("IX_Posts_Moderation_Work");

            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.AuthorId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
