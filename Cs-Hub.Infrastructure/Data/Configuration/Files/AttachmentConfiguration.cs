using Cs_Hub.Domain.Enums;
using AttachmentEntity = Cs_Hub.Domain.entities.Files.Attachments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Files
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<AttachmentEntity>
    {
        public void Configure(EntityTypeBuilder<AttachmentEntity> builder)
        {
            builder.ToTable("Attachments", t =>
            {
                t.HasCheckConstraint("CK_Attachments_Purpose", "[Purpose] IN (1, 2)");
                t.HasCheckConstraint("CK_Attachments_Avatar_NoPost", "[Purpose] <> 2 OR [PostId] IS NULL");
                t.HasCheckConstraint("CK_Attachments_Size_Positive", "[Size] > 0");
                t.HasCheckConstraint("CK_Attachments_ScanStatus", "[ScanStatus] IN (0, 1, 2, 3)");
                t.HasCheckConstraint("CK_Attachments_Width_Positive", "[Width] IS NULL OR [Width] > 0");
                t.HasCheckConstraint("CK_Attachments_Height_Positive", "[Height] IS NULL OR [Height] > 0");
                t.HasCheckConstraint("CK_Attachments_PageCount_Positive", "[PageCount] IS NULL OR [PageCount] > 0");
            });

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedNever();
            builder.Property(a => a.UserId).IsRequired();
            builder.Property(a => a.Purpose)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .HasDefaultValue(AttachmentPurpose.PostAttachment)
                .IsRequired();
            builder.Property(a => a.FileName)
                .HasMaxLength(255)
                .IsRequired();
            builder.Property(a => a.ContentType)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(a => a.Size)
                .IsRequired();
            builder.Property(a => a.StorageKey)
                .HasMaxLength(512)
                .IsRequired();
            builder.Property(a => a.ScanStatus)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .HasDefaultValue(ScanStatus.Pending)
                .IsRequired();
            builder.Property(a => a.ScannedAt)
                .HasColumnType("datetime2(3)");
            builder.Property(a => a.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(a => a.Post)
                .WithMany(p => p.Attachments)
                .HasForeignKey(a => a.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.StorageKey)
                .IsUnique()
                .HasDatabaseName("UX_Attachments_StorageKey");
            builder.HasIndex(a => new { a.UserId, a.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("IX_Attachments_Owner_Created");
            builder.HasIndex(a => a.CreatedAt)
                .HasFilter("[PostId] IS NULL AND [Purpose] = 1")
                .HasDatabaseName("IX_Attachments_Orphans");

        }
    }
}
