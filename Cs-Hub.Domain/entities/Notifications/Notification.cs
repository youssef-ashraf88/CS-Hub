using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Social;
using Cs_Hub.Domain.entities.Users;
using Cs_Hub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid RecipientUserId { get; set; }
        public Guid? ActorUserId { get; set; }
        public NotificationType Type { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }

        //Navigation properties
        public ApplicationUser Recipient { get; set; } = null!;
        public ApplicationUser? Actor { get; set; }
        public Post? Post { get; set; }
        public Comment? Comment { get; set; }
    }
}
