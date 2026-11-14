using Cs_Hub.Domain.entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Social
{
    public class SavedPostConfiguration : IEntityTypeConfiguration<SavedPost>
    {
        public void Configure(EntityTypeBuilder<SavedPost> builder)
        {
            builder.ToTable("SavedPosts");
            
            builder.HasKey(s => new { s.PostId, s.UserId });
            builder.Property(s => s.SavedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasOne(s => s.Post)
                .WithMany(p => p.SavedPosts)
                .HasForeignKey(s => s.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(s => new { s.UserId, s.SavedAt, s.PostId })
                .IsDescending(false, true, false)
                .HasDatabaseName("IX_SavedPosts_User_Saved");
        }
    }
}
