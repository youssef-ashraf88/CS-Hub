using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Social
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        //Navigation properties
        public Post Post { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
