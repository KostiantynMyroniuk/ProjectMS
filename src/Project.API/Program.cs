using Project.API.Apis.Invitations;
using Project.API.Apis.Projects;
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

app.MapInvitationApi();

app.Run();
