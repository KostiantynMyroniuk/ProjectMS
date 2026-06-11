using Email.Worker.Services;
using MassTransit;
using Shared.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Email.Worker.Consumers
{
    public class InvitationCreatedConsumer(
        IEmailService emailService,
        ILogger<InvitationCreatedConsumer> logger) : IConsumer<InvitationCreatedEvent>
    {
        public async Task Consume(ConsumeContext<InvitationCreatedEvent> context)
        {
            var ev = context.Message;

            logger.LogInformation("Sending invitation email to {Email} for project {Project}", ev.Email, ev.ProjectName);

            await emailService.SendInvitationAsync(
                toEmail: ev.Email,
                projectName: ev.ProjectName,
                invitedByName: ev.InvitedByName,
                acceptUrl: ev.AcceptUrl,
                expiresAt: ev.ExpiresAt);

        }
    }
}
