using MassTransit;
using Microsoft.EntityFrameworkCore;
using Project.API.Infrastructure;
using Project.API.Models;
using Shared.Services;
using System.Text.Json.Serialization;

namespace Project.API.Extensions
{
    public static class Extensions
    {
        public static void AddProjectConfiguration(this IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("projectDb"));
            });

            builder.Services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(builder.Configuration.GetConnectionString("rabbitmq"));

                    cfg.ConfigureEndpoints(context);
                });
            });

            builder.Services.ConfigureHttpJsonOptions(options =>
            {
                options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization();


            builder.Services.Configure<AppOptions>(builder.Configuration.GetSection("App"));

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
