using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.DTO
{
    public class RegisterResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public bool EmailSent { get; set; }
    }
}
