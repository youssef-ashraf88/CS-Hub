using Cs_Hub.Domain.entities.Content;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Infrastructure.Data.Configuration.Content
{
    public class TopicConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd();
            builder.Property(t => t.Name).IsRequired().HasMaxLength(80);
            builder.Property(t => t.Slug).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).HasMaxLength(300);
            builder.Property(t => t.IsActive)
                .HasDefaultValue(true)
                .IsRequired();
            builder.Property(t => t.CreatedAt)
                .HasColumnType("datatime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasIndex(t => t.Name).IsUnique().HasDatabaseName("UX_Topics_Name");
            builder.HasIndex(t => t.Slug).IsUnique().HasDatabaseName("UX_Topics_Slug");
        }
    }
}
