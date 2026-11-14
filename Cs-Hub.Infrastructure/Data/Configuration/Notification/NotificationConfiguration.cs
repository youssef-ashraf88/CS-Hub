using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationEntity = Cs_Hub.Domain.entities.Notifications.Notification;

namespace Cs_Hub.Infrastructure.Data.Configuration.Notification
{
    public class NotificationConfiguration : IEntityTypeConfiguration<NotificationEntity>
    {
        public void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.ToTable("Notifications", t =>
            {
                t.HasCheckConstraint("CK_Notifications_Type", "[Type] IN (1, 2, 3, 4, 5, 6, 7)");
                t.HasCheckConstraint("CK_Notifications_NewFollower_NoPost", "[Type] <> 1 OR [PostId] IS NULL");
                t.HasCheckConstraint("CK_Notifications_PostTypes_HavePost", "[Type] <> 1 OR [PostId] IS NOT NULL");
                t.HasCheckConstraint("CK_Notifications_PostComment_HasComment", "[Type] <> 3 OR [CommentId] IS NOT NULL");
            });

            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).ValueGeneratedOnAdd();
            builder.Property(n => n.RecipientUserId).IsRequired();
            builder.Property(n => n.Type)
                .HasConversion<byte>()
                .HasColumnType("tinyint")
                .IsRequired();
            builder.Property(n => n.ReadAt).HasColumnType("datetime2(3)");
            builder.Property(n => n.CreatedAt)
                .HasColumnType("datetime2(3)")
                .HasDefaultValueSql("SYSUTCDATETIME()")
                .IsRequired();

            builder.HasOne(n => n.Recipient)
                .WithMany()
                .HasForeignKey(n => n.RecipientUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Actor)
                .WithMany()
                .HasForeignKey(n => n.ActorUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Comment)
                .WithMany()
                .HasForeignKey(n => n.CommentId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(n => n.Post)
                .WithMany()
                .HasForeignKey(n => n.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(n => new { n.RecipientUserId, n.ActorUserId })
                .IsUnique()
                .HasFilter("[Type] = 1")
                .HasDatabaseName("UX_Notif_NewFollower ");
            builder.HasIndex(n => new { n.RecipientUserId, n.ActorUserId, n.PostId })
                .IsUnique()
                .HasFilter("[Type] = 2")
                .HasDatabaseName("UX_Notif_PostReaction");
            builder.HasIndex(n => new { n.RecipientUserId, n.Id })
                .IsUnique()
                .IsDescending(false, true)
                .HasFilter("[Type] = 3")
                .HasDatabaseName("UC_Notif_PostComment");
            builder.HasIndex(n => new { n.RecipientUserId, n.Id })
                .IsDescending(false, true)
                .HasDatabaseName("IX_Notif_Recipient");
            builder.HasIndex(n => n.RecipientUserId)
                .HasFilter("[ReadAt] IS NULL")
                .HasDatabaseName("IX_Notif_Unread");
        }           
    }
}
