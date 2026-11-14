using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.DTO
{
    public class LoginResponseDTO
    {
        public string? Email { get; set; } 
        public IEnumerable<string>? Role {  get; set; }
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
