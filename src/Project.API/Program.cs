using Project.API.Apis;
using Project.API.Extensions;
using Shared.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.AddProjectConfiguration();

builder.Services.AddJwtCookieAuth(builder.Configuration);

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.UseMigrations();

app.MapProjectApi();

app.Run();
