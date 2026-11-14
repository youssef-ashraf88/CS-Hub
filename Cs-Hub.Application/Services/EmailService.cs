using Cs_Hub.Application.ServiceContracts;
using Cs_Hub.Application.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cs_Hub.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("CS Hub", _smtpSettings.Email));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(
                _smtpSettings.Host,
                _smtpSettings.Port,
                SecureSocketOptions.StartTls
                );
                await client.AuthenticateAsync(_smtpSettings.Email, _smtpSettings.AppPassword);
                await client.SendAsync(message);
            }
            finally
            {
                if(client.IsConnected)
                    await client.DisconnectAsync(true);
            }
        }
    }
}
