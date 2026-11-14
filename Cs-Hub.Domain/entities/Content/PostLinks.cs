using Cs_Hub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Content
{
    public class PostLinks
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public LinkKind LinkKind { get; set; } = LinkKind.Web;
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }

        //Navigation properties
        public Post Post { get; set; } = null!;
    }
}
