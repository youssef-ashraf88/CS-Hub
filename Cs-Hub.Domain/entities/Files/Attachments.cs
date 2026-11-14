using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Moderation;
using Cs_Hub.Domain.entities.Users;
using Cs_Hub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cs_Hub.Domain.entities.Files
{
    public class Attachments
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public AttachmentPurpose Purpose { get; set; } = AttachmentPurpose.PostAttachment;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
        public string StorageKey { get; set; } = string.Empty;
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? PageCount { get; set; }
        public ScanStatus ScanStatus { get; set; } = ScanStatus.Pending;
        public DateTime CreatedAt { get; set; }
        public DateTime ScannedAt { get; set; }

        //Navigation properties
        public ApplicationUser User { get; set; } = null!;
        public Post Post { get; set; } = null!;
        public ICollection<ModerationDecision> ModerationDecisions { get; set; } = new List<ModerationDecision>();
    }
}
