using Microsoft.EntityFrameworkCore;
using Project.API.Infrastructure;
using Project.API.Services;

namespace Project.API.Extensions
{
    public static class Extensions
    {
        public static void AddProjectConfiguration(this IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("projectmsDb"));
            });

            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddAuthorization();
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
