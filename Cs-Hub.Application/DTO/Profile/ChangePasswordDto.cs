using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.DTO.Profile
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword {  get; set; } = string.Empty;
    }
}
