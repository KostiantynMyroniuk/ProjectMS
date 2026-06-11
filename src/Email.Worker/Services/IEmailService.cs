using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Worker.Services
{
    public interface IEmailService
    {
        Task SendInvitationAsync(string toEmail, string projectName, string invitedByName, string acceptUrl, DateTime expiresAt, CancellationToken ct = default);
    }
}
