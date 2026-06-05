var builder = DistributedApplication.CreateBuilder(args);

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

var sqlserver = builder.AddSqlServer("projectmsSqlServer", null, 60000)
    .AddDatabase("projectmsDb");

builder.AddProject<Projects.Identity_API>("identity-api")
    .WithReference(sqlserver)
    .WithEnvironment("Jwt__Key", jwtKey)
    .WithEnvironment("Jwt__Issuer", jwtIssuer)
    .WithEnvironment("Jwt__Audience", jwtAudience)
    .WaitFor(sqlserver);

builder.AddProject<Projects.Project_API>("project-api")
    .WithReference(sqlserver)
    .WithEnvironment("Jwt__Key", jwtKey)
    .WithEnvironment("Jwt__Issuer", jwtIssuer)
    .WithEnvironment("Jwt__Audience", jwtAudience)
    .WaitFor(sqlserver);

builder.AddProject<Projects.Task_API>("task-api")
    .WaitFor(sqlserver);

builder.Build().Run();
