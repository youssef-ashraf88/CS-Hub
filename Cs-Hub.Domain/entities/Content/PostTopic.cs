using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Content
{
    public class PostTopic
    {
        public Guid PostId { get; set; }
        public Guid TopicId { get; set; }

        //Navigation properties
        public Post Post { get; set; } = null!;
        public Topic Topic { get; set; } = null!;
    }
}
