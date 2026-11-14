using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Domain.entities.Content
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        //Navigation properties
        public ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();
    }
}
