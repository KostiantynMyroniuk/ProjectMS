using File.API.Consumers;
using File.API.Infrastructure;
using File.API.Models;
using File.API.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Shared.Auth;
using Shared.Services;

namespace File.API.Extensions
{
    public static class Extension
    {
        public static void AddConfiguration(this IHostApplicationBuilder builder)
        {
            //external services
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("fileDb"));
            });

            builder.Services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.AddBlobServiceClient(builder.Configuration["BLOBS_CONNECTIONSTRING"]);
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

            //Auth
            builder.Services.AddJwtCookieAuth(builder.Configuration);

            builder.Services.AddAuthorization();

            builder.Services.AddHttpContextAccessor();

            //services and options
            builder.Services.Configure<BlobOptions>(builder.Configuration.GetSection("FileStorage"));

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddTransient<IFileStorageService, FileStorageService>();
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
