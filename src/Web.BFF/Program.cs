using Web.BFF.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfigurations();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapReverseProxy();

app.Run();

