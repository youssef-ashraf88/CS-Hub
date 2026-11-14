using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Users;
using Cs_Hub.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Social
{
    public class Reaction
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public ReactionType Type { get; set; } = ReactionType.Like;
        public DateTime CreatedAt { get; set; }

        //Navigation properties
        public Post Post { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
