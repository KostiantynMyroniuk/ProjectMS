using Email.Worker.Models;
using Email.Worker.Services;
using Email.Worker.Services.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Moq;

namespace EmailWorker.UnitTests
{
    public class EmailServiceTests
    {
        private readonly Mock<ISmtpClientWrapper> _smtpClientMock = new();
        private readonly Mock<ISmtpClientFactory> _smtpFactoryMock = new();
        private readonly Mock<ILogger<EmailSenderService>> _loggerMock = new();
        private readonly EmailSenderService _emailService;

        public EmailServiceTests()
        {
            _smtpFactoryMock.Setup(f => f.Create()).Returns(_smtpClientMock.Object);

            var options = Options.Create(new EmailOptions
            {
                FromEmail = "noreply@gmail.com",
                Host = "smtp.test.com",
                Port = 587,
                Password = "securePass"
            });

            _emailService = new EmailSenderService(_smtpFactoryMock.Object, options, _loggerMock.Object);
        }

        [Fact]
        public async Task SendInvitationAsync_ShouldConnectAndSendMessage()
        {
            _smtpClientMock.Setup(c => c.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _smtpClientMock.Setup(c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _smtpClientMock.Setup(c => c.SendAsync(It.IsAny<MimeMessage>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);


            await _emailService.SendInvitationAsync(
                "user@example.com",
                "Project1",
                "User1",
                "https://app.com/accept/123",
                DateTime.UtcNow.AddDays(7));

            _smtpClientMock.Verify(c =>
            c.ConnectAsync("smtp.test.com", 587,
                SecureSocketOptions.StartTls, default), Times.Once);

            _smtpClientMock.Verify(c =>
                c.SendAsync(It.IsAny<MimeMessage>(), default), Times.Once);

            _smtpClientMock.Verify(c =>
                c.DisconnectAsync(true, default), Times.Once);
        }

        
    }
}
