using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Worker.Services.Smtp
{
    public interface ISmtpClientWrapper : IAsyncDisposable
    {
        Task ConnectAsync(string host, int port, SecureSocketOptions options, CancellationToken ct);
        Task AuthenticateAsync(string userName, string password, CancellationToken ct);
        Task SendAsync(MimeMessage message, CancellationToken ct);
        Task DisconnectAsync(bool quit, CancellationToken ct);
    }
}
