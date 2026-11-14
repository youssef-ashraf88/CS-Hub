using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.DTO
{
    public class RegisterDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
