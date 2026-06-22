using ProjectMS.AppHost.Configs;

var builder = DistributedApplication.CreateBuilder(args);

var jwtKey = builder.AddParameter("jwt-key", builder.Configuration["Jwt:Key"], secret: true);
var jwtIssuer = builder.AddParameter("jwt-issuer", builder.Configuration["Jwt:Issuer"]);
var jwtAudience = builder.AddParameter("jwt-audience", builder.Configuration["Jwt:Audience"]);

var sqlserver = builder.AddSqlServer("projectmsSqlServer", null, 60000);

var identityDb = sqlserver.AddDatabase("identityDb");
var projectDb = sqlserver.AddDatabase("projectDb");
var taskDb = sqlserver.AddDatabase("taskDb");
var fileDb = sqlserver.AddDatabase("fileDb");

var rabbitmq = builder.AddRabbitMQ("rabbitmq");

var azureStorage = builder.AddAzureStorage("azure-storage")
    .RunAsEmulator();

var blobs = azureStorage.AddBlobs("blobs");

builder.AddProject<Projects.Identity_API>("identity-api")
    .WithJwtConfig(jwtKey, jwtIssuer, jwtAudience)
    .WithReference(identityDb).WaitFor(identityDb);

var projectApi = builder.AddProject<Projects.Project_API>("project-api")
    .WithJwtConfig(jwtKey, jwtIssuer, jwtAudience)
    .WithReference(projectDb).WaitFor(projectDb)
    .WithReference(rabbitmq).WaitFor(rabbitmq);

builder.AddProject<Projects.Tasks_API>("tasks-api")
    .WithJwtConfig(jwtKey, jwtIssuer, jwtAudience)
    .WithReference(projectApi)
    .WithReference(taskDb).WaitFor(taskDb)
    .WithReference(rabbitmq).WaitFor(rabbitmq);

builder.AddProject<Projects.File_API>("file-api")
    .WithJwtConfig(jwtKey, jwtIssuer, jwtAudience)
    .WithReference(fileDb).WaitFor(fileDb)
    .WithReference(rabbitmq).WaitFor(rabbitmq)
    .WithReference(blobs).WaitFor(blobs);

builder.AddProject<Projects.Email_Worker>("email-worker")
    .WithReference(rabbitmq)
    .WaitFor(rabbitmq);

builder.Build().Run();


