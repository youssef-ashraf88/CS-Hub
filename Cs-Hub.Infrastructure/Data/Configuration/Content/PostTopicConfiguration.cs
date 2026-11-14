using Cs_Hub.Domain.entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Content
{
    public class PostTopicConfiguration : IEntityTypeConfiguration<PostTopic>
    {
        public void Configure(EntityTypeBuilder<PostTopic> builder)
        {
            builder.ToTable("PostTopics");
            builder.HasKey(pt => new { pt.PostId, pt.TopicId });

            builder.HasOne(pt => pt.Post)
                .WithMany(p => p.PostTopics)
                .HasForeignKey(pt => pt.PostId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pt => pt.Topic)
                .WithMany(t => t.PostTopics)
                .HasForeignKey(pt => pt.TopicId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(pt => new { pt.TopicId, pt.PostId })
                .HasDatabaseName("IX_PostTopics_Topic_Post");
        }
    }
}
