using Cs_Hub.Domain.entities.Social;
using Cs_Hub.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Social
{
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.ToTable("Reactions", t =>
            {
                t.HasCheckConstraint("CK_Reactions_Type", "[Type] IN (1)");
            });

            builder.HasKey(r => new { r.PostId, r.UserId });
            builder.Property(r => r.Type)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .HasDefaultValue(ReactionType.Like)
                .IsRequired();
            builder.Property(r => r.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
