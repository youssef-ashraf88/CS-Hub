using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Settings
{
    public class SmtpSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Email { get; set; } = string.Empty;
        public string AppPassword { get; set; } = string.Empty;
    }
}
