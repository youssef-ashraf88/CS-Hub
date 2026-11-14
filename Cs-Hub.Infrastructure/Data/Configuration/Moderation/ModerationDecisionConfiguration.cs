using Cs_Hub.Domain.entities.Moderation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Moderation
{
    public class ModerationDecisionConfiguration : IEntityTypeConfiguration<ModerationDecision>
    {
        public void Configure(EntityTypeBuilder<ModerationDecision> builder)
        {
            builder.ToTable("ModerationDecisions", t =>
            {
                t.HasCheckConstraint("CK_ModerationDecisions_Source", "[Source] IN (1, 2)");
                t.HasCheckConstraint("CK_ModerationDecisions_Scope", "[Scope] IN (1, 2, 3, 4, 5)");
                t.HasCheckConstraint("CK_ModerationDecisions_Decision", "[Decision] IN (1, 2, 3, 4, 5)");
                t.HasCheckConstraint("CK_ModerationDecisions_Source_Decision", "([Source] = 1 AND [Decision] IN (1, 2, 3, 4)) OR ([Source] = 2 AND [Decision] IN (1, 2, 5))");
                t.HasCheckConstraint("CK_ModerationDecisions_Reviewer", "([Source] = 1 AND [ReviewerUserId] IS NULL) OR ([Source] = 2 AND [ReviewerUserId] IS NOT NULL)");
                t.HasCheckConstraint("CK_ModerationDecisions_Ai_Has_Provider", "[Source] <> 1 OR [Provider] IS NOT NULL");
                t.HasCheckConstraint("CK_ModerationDecisions_Confidence_Range", "[Confidence] IS NULL OR [Confidence] BETWEEN 0 AND 1");
            });

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).ValueGeneratedNever();
            builder.Property(m => m.PostId).IsRequired();
            builder.Property(m => m.Source)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .IsRequired();
            builder.Property(m => m.Scope)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .IsRequired();
            builder.Property(m => m.Decision)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .IsRequired();
            builder.Property(m => m.Provider).HasMaxLength(50);
            builder.Property(m => m.Model).HasMaxLength(100);
            builder.Property(m => m.PolicyVersion).HasMaxLength(20);
            builder.Property(m => m.Confidence).HasPrecision(4, 3);
            builder.Property(m => m.ReasonCode).HasMaxLength(50);
            builder.Property(m => m.Explanation).HasMaxLength(1000);
            builder.Property(m => m.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasOne(m => m.Post)
                .WithMany(p => p.ModerationDecisions)
                .HasForeignKey(m => m.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(m => m.Attachment)
                .WithMany(a => a.ModerationDecisions)
                .HasForeignKey(m => m.AttachmentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.ReviewerUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(m => new { m.PostId, m.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("IX_ModDec_Post_Created");
            builder.HasIndex(m => new { m.ReviewerUserId, m.CreatedAt })
                .IsDescending(false, true)
                .HasFilter("[ReviewerUserId] IS NOT NULL")
                .HasDatabaseName("IX_ModDec_Reviewer_Created");
        }
    }
}
