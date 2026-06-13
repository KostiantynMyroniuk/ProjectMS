using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Worker.Services.Smtp
{
    public class SmtpClientWrapper : ISmtpClientWrapper
    {
        private readonly SmtpClient _smtpClient = new();

        public Task AuthenticateAsync(string userName, string password, CancellationToken ct)
        {
            return _smtpClient.AuthenticateAsync(userName, password, ct);
        }

        public Task ConnectAsync(string host, int port, SecureSocketOptions options, CancellationToken ct)
        {
            return _smtpClient.ConnectAsync(host, port, options, ct);
        }

        public Task DisconnectAsync(bool quit, CancellationToken ct)
        {
            return _smtpClient.DisconnectAsync(quit, ct);
        }

        public Task SendAsync(MimeMessage message, CancellationToken ct)
        {
            return _smtpClient.SendAsync(message, ct);
        }

        public ValueTask DisposeAsync() 
        {
            _smtpClient.Dispose();
            return ValueTask.CompletedTask;
        } 
    }
}
