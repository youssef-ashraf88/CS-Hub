using Cs_Hub.Domain.entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Social
{
    public class Follow
    {
        public Guid FollowerId { get; set; }
        public Guid FolloweeId { get; set; }
        public DateTime FollowedAt { get; set; }

        //Navigation properties
        public ApplicationUser Follower { get; set; } = null!;
        public ApplicationUser Followee { get; set; } = null!;
    }
}
