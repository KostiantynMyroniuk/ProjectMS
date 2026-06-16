using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System.Text.Json.Serialization;
using Tasks.API.Consumers;
using Tasks.API.Infrastructure;

namespace Tasks.API.Extensions
{
    public static class Extension
    {
        public static void AddAppConfiguration(this IHostApplicationBuilder builder)
        {
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddAuthorization();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("projectmsDb"));
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumer<UserAddedToProjectConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));

                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.AddScoped<IIdentityService, IdentityService>();
        }

        public static void UseMigrations(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }
        }
    }
}
