using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AttachmentEntity = Cs_Hub.Domain.entities.Files.Attachments;
using System;
using System.Collections.Generic;
using System.Text;
using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Moderation;
using Cs_Hub.Domain.entities.Notifications;
using Cs_Hub.Domain.entities.Social;
using Cs_Hub.Domain.entities.Users;

namespace Cs_Hub.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<PostLinks> PostLinks => Set<PostLinks>();
        public DbSet<PostTag> PostTags => Set<PostTag>();
        public DbSet<PostTopic> PostTopics => Set<PostTopic>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Topic> Topics => Set<Topic>();
        public DbSet<Comment> Comments => Set<Comment>();
        public DbSet<Follow> Follows => Set<Follow>();
        public DbSet<Reaction> Reactions => Set<Reaction>();
        public DbSet<SavedPost> SavedPosts => Set<SavedPost>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<AttachmentEntity> Attachments => Set<AttachmentEntity>();
        public DbSet<ModerationDecision> ModerationDecisions => Set<ModerationDecision>();
        public DbSet<Report> Reports => Set<Report>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ApplicationDbContext).Assembly);
        }
    }
}
