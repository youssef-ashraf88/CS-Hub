using System;
using System.Collections.Generic;
using AttachmentEntity = Cs_Hub.Domain.entities.Files.Attachments;
using System.Text;

namespace Cs_Hub.Domain.entities.Users
{
    public class UserProfile
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? HeadLine { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? WebsiteUrl { get; set; }
        public Guid? AvatarAttachmentId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime? UpdatedAt { get; set; }

        //Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public AttachmentEntity? AvatarAttachment { get; set; }
    }
}
