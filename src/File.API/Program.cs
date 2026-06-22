using File.API.Apis;
using File.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

app.UseMigrations();

app.MapFileApi();

app.Run();

