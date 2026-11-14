using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Content
{
    public class Topic
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }

        //Navigation properties
        public ICollection<PostTopic> PostTopics { get; set; } = new List<PostTopic>();
    }
}
