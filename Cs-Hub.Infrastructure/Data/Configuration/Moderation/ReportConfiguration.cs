using Cs_Hub.Domain.entities.Moderation;
using Cs_Hub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Moderation
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("Reports", t =>
            {
                t.HasCheckConstraint("CK_Reports_Reason", "[Reason] IN (1, 2, 3, 4, 5)");
                t.HasCheckConstraint("CK_Reports_Status", "[Status] IN (1, 2, 3)");
                t.HasCheckConstraint("CK_Reports_Exactly_One_Target", "([PostId] IS NULL AND [CommentId] IS NOT NULL) OR ([PostId] IS NOT NULL AND [CommentId] IS NULL)");
                t.HasCheckConstraint("CK_Reports_Resolution_Shape", "([Status] = 1 AND [ResolvedAt] IS NULL AND [ResolvedByUserId] IS NULL) OR ([Status] IN (2, 3) AND [ResolvedAt] IS NOT NULL AND [ResolvedByUserId] IS NOT NULL)");
            });

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedNever();
            builder.Property(r => r.ReporterId).IsRequired();
            builder.Property(r => r.Reason)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .IsRequired();
            builder.Property(r => r.Description).HasMaxLength(1000);
            builder.Property(r => r.Status)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .HasDefaultValue(ReportStatus.Pending)
                .IsRequired();
            builder.Property(r => r.ResolvedAt)
                .HasColumnType("datetime2(3)");
            builder.Property(r => r.ResolutionNote).HasMaxLength(1000);
            builder.Property(r => r.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasOne(r => r.Post)
                .WithMany()
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.Reporter)
                .WithMany()
                .HasForeignKey(r => r.ReporterId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.Comment)
                .WithMany()
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(r => r.ResolvedBy)
                .WithMany()
                .HasForeignKey(r => r.ResolvedByUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(r => new { r.ReporterId, r.PostId })
                .IsUnique()
                .HasFilter("[PostId] IS NOT NULL")
                .HasDatabaseName("UX_Reports_Reporter_Post");
            builder.HasIndex(r => new { r.ReporterId, r.CommentId })
                .IsUnique()
                .HasFilter("[CommentId] IS NOT NULL")
                .HasDatabaseName("UX_Reports_Reporter_Comment");
            builder.HasIndex(r => r.CreatedAt)
                .HasFilter("[Status] = 1")
                .HasDatabaseName("IX_Reports_Open");
            builder.HasIndex(r => r.PostId)
                .HasFilter("[PostId] IS NOT NULL")
                .HasDatabaseName("IX_Reports_Post");
        }
    }
}
