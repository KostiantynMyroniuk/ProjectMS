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
            //external services
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("taskDb"));
            });

            builder.Services.AddMassTransit(x =>
            {
                x.AddConsumers(typeof(ProjectCreatedConsumer).Assembly);

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));

                    cfg.ConfigureEndpoints(context);
                });
            });

            //auth and http options
            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddAuthorization();

            builder.Services.AddHttpContextAccessor();

            //services
            builder.Services.AddScoped<IIdentityService, IdentityService>();
        }

        //migrations for dev
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
