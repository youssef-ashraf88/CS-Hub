using Cs_Hub.Domain.entities.Social;
using Cs_Hub.Domain.entities.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Social
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments", t =>
            {
                t.HasCheckConstraint("CK_Comments_Body_NotBlank", "LEN(LTRIM(RTRIM([Body]))) > 0");
            });

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.PostId).IsRequired();
            builder.Property(c => c.UserId).IsRequired();
            builder.Property(c => c.Body).IsRequired().HasMaxLength(2000);
            builder.Property(c => c.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();
            builder.Property(c => c.UpdatedAt).HasColumnType("datetime2(3)");
            builder.Property(c => c.DeletedAt).HasColumnType("datetime2(3)");

            builder.HasQueryFilter(c => c.DeletedAt == null);

            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(c => new { c.PostId, c.CreatedAt, c.Id })
                .HasFilter("[DeletedAt] IS NULL")
                .HasDatabaseName("IX_Comments_Post_Created");
            builder.HasIndex(c => new { c.UserId, c.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("IC_Comments_Author_Created");
        }
    }
}
