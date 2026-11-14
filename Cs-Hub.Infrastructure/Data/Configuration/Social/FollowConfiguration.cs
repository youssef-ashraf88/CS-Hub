using Cs_Hub.Domain.entities.Social;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Social
{
    public class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.ToTable("Follows", t =>
            {
                t.HasCheckConstraint("CK_Follows_No_SelfFollow", "[FollowerId] <> [FolloweeId]");
            });

            builder.HasKey(f => new { f.FollowerId, f.FolloweeId });
            builder.Property(f => f.FollowedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(f => f.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FolloweeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(f => new { f.FolloweeId, f.FollowerId })
                .HasDatabaseName("IX_Follows_Followee_Follower");

            builder.HasIndex(f => new { f.FollowerId, f.FollowedAt })
                .IsDescending(false, true)
                .IncludeProperties(f => f.FolloweeId)
                .HasDatabaseName("IX_Follows_Follower_Created");
        }
    }
}
