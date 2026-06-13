using Email.Worker.Consumers;
using Email.Worker.Models;
using Email.Worker.Services;
using Email.Worker.Services.Smtp;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));

builder.Services.AddScoped<IEmailService, EmailSenderService>();

builder.Services.AddSingleton<ISmtpClientFactory, SmtpClientFactory>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<InvitationCreatedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));

        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();
