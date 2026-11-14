using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Social;
using Cs_Hub.Domain.entities.Users;
using Cs_Hub.Domain.Enums;
using Cs_Hub.Domain.entities.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Moderation
{
    public class Report
    {
        public Guid Id { get; set; }
        public Guid ReporterId { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentId { get; set; }
        public ReportReason Reason { get; set; }
        public string? Description { get; set; }
        public ReportStatus Status { get; set; }
        public Guid? ResolvedByUserId { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? ResolutionNote { get; set; }
        public DateTime CreatedAt { get; set; }

        //Navigation properties
        public ApplicationUser Reporter { get; set; } = null!;
        public ApplicationUser? ResolvedBy { get; set; }
        public Post? Post { get; set; }
        public Comment? Comment { get; set; }
    }
}
