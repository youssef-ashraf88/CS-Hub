using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.ServiceContracts
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}