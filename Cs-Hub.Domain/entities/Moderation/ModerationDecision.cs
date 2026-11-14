using Cs_Hub.Domain.Enums;
using AttachmentEntity = Cs_Hub.Domain.entities.Files.Attachments;
using System;
using System.Collections.Generic;
using System.Text;
using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Users;

namespace Cs_Hub.Domain.entities.Moderation
{
    public class ModerationDecision
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid? AttachmentId { get; set; }
        public ModerationSource Source { get; set; }
        public ModerationScope Scope { get; set; }
        public ModerationDecisionType Decision { get; set; }
        public Guid? ReviewerUserId { get; set; }
        public string? Provider { get; set; }
        public string? Model { get; set; }
        public string? PolicyVersion { get; set; } //cs-hub policy version
        public decimal? Confidence { get; set; }
        public string? ReasonCode { get; set; }
        public string? Explanation { get; set; }
        public int? Latency { get; set; }
        public DateTime CreatedAt { get; set; }

        //Navigation properties
        public Post Post { get; set; } = null!;
        public AttachmentEntity? Attachment { get; set; } = null!;
        public ApplicationUser? User { get; set; }
    }
}
