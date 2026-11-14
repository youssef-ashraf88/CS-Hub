using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.DTO.Profile
{
    public class UpdateProfileDto
    {
        public string DisplayName { get; set; } = string.Empty;
        public string? HeadLine { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? WebsiteUrl { get; set; }
    }
}
