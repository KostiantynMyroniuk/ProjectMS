using Shared.Auth;
using Tasks.API.Apis;
using Tasks.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtCookieAuth(builder.Configuration);

builder.AddAppConfiguration();

var app = builder.Build();

app.UseMigrations();

app.UseAuthentication();

app.UseAuthorization();

app.MapTaskApi();

app.Run();

