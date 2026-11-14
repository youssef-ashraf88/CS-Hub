using Cs_Hub.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using AttachmentEntity = Cs_Hub.Domain.entities.Files.Attachments;

using System.Text;
using Cs_Hub.Domain.entities.Content;
using Cs_Hub.Domain.entities.Social;

namespace Cs_Hub.Domain.entities.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Navigation properties
        public UserProfile? UserProfile { get; set; }   
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
        public ICollection<Follow> Following { get; set; } = new List<Follow>();
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
    }
}
