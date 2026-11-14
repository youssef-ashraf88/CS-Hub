using Cs_Hub.Domain.Enums;
using AttachmentEntity = Cs_Hub.Domain.entities.Files.Attachments;
using System;
using System.Collections.Generic;
using System.Text;
using Cs_Hub.Domain.entities.Moderation;
using Cs_Hub.Domain.entities.Social;
using Cs_Hub.Domain.entities.Users;

namespace Cs_Hub.Domain.entities.Content
{
    public class Post
    {
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public string? Title { get; set; }
        public string Body { get; set; } = string.Empty;
        public ModerationStatus ModerationStatus { get; set; } = ModerationStatus.Pending;
        public byte ModerationAttempts { get; set; } = 0;
        public DateTime PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        //Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public ICollection<PostLinks> PostLinks { get; set; } = new List<PostLinks>();
        public ICollection<PostTopic> PostTopics { get; set; } = new List<PostTopic>();
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
        public ICollection<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
        public ICollection<ModerationDecision> ModerationDecisions { get; set; } = new List<ModerationDecision>();
    }
}
