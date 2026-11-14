using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Content
{
    public class PostLinkConfiguration : IEntityTypeConfiguration<PostLinks>
    {
        public void Configure(EntityTypeBuilder<PostLinks> builder)
        {
            builder.ToTable("PostLinks", t =>
            {
                t.HasCheckConstraint("CK_PostLinks_LinkKind", "LinkKind] IN (1, 2)");
            });

            builder.HasKey(l => l.Id);
            builder.Property(l => l.Id).ValueGeneratedNever();
            builder.Property(l => l.PostId).IsRequired();
            builder.Property(l => l.Url).IsRequired().HasMaxLength(2048);
            builder.Property(l => l.Host).IsRequired().HasMaxLength(255);
            builder.Property(l => l.LinkKind)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .HasDefaultValue(LinkKind.Web)
                .IsRequired();
            builder.Property(l => l.Title).HasMaxLength(300);
            builder.Property(l => l.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasIndex(l => l.Host).HasDatabaseName("IX_PostLinks_Host");

            builder.HasOne(l => l.Post)
                .WithMany(p => p.PostLinks)
                .HasForeignKey(l => l.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
