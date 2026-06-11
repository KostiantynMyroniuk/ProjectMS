using Email.Worker.Models;
using MailKit;
using MailKit.Net.Smtp;
using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Worker.Services
{
    public class EmailSenderService(
        IOptions<EmailOptions> options, 
        ILogger<EmailSenderService> logger) : IEmailService
    {
        public async Task SendInvitationAsync(
            string toEmail, 
            string projectName, 
            string invitedByName, 
            string acceptUrl, 
            DateTime expiresAt, 
            CancellationToken ct = default)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(options.Value.FromEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = $"You have been invited to join {projectName}";

            var body = new BodyBuilder
            {
                HtmlBody = $"""
                <h2>Project invitation</h2>
                <p><strong>{invitedByName}</strong> has invited you to join
                   the project <strong>{projectName}</strong>.</p>
                <p>
                  <a href="{acceptUrl}"
                     style="padding:10px 20px;background:#1a2535;color:#fff;
                            text-decoration:none;border-radius:6px">
                    Accept invitation
                  </a>
                </p>
                <p style="color:#888;font-size:12px">
                  Link expires: {expiresAt:yyyy-MM-dd HH:mm} UTC
                </p>
                """
            };

            message.Body = body.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(options.Value.Host, options.Value.Port, MailKit.Security.SecureSocketOptions.StartTls, ct);
            await client.AuthenticateAsync(options.Value.FromEmail, options.Value.Password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);

            logger.LogInformation("Invitation email sent to {Email} for project {Project}", toEmail, projectName);
        }
    }
}
