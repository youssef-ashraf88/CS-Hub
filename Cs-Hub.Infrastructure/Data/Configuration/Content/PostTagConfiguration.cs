using Cs_Hub.Domain.entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Content
{
    public class PostTagConfiguration : IEntityTypeConfiguration<PostTag>
    {
        public void Configure(EntityTypeBuilder<PostTag> builder)
        {
            builder.ToTable("PostTags");
            builder.HasKey(pt => new { pt.PostId, pt.TagId });
            builder.HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(pt => new { pt.TagId, pt.PostId })
                .HasDatabaseName("IX_PostTags_Tag_Post");
        }
    }
}
