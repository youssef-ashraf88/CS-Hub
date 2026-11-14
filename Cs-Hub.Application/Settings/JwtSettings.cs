using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Settings
{
    public class JwtSettings
    {
        public string? Issuer { get; set; }
        public string? Key { get; set; }
        public string? Audience { get; set; }
        public int EXPIRATION_MINUTES { get; set; }
    }
}
