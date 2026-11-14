using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Social
{
    public class SavedPost
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public DateTime SavedAt { get; set; }

        //Navigation properties
        public Post Post { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
