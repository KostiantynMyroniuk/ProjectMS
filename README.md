# ProjectMS

Project Management System — a microservices-based backend application for managing projects, tasks, files, and member invitations.

## Architecture

The application is built on .NET Aspire and consists of several independent services that communicate with each other via messaging (RabbitMQ / MassTransit).

| Service | Description |
|---|---|
| `Identity.API` | Registration, login, JWT issuance |
| `Project.API` | Project creation, members, invitations |
| `Tasks.API` | Tasks within a project |
| `File.API` | Uploading and storing task files |
| `Email.Worker` | Background worker that sends email invitations |
| `ProjectMS.AppHost` | Aspire orchestrator: spins up infrastructure and all services locally |
| `Shared` | Shared modules |

Each API service has its own database (SQL Server):

- `identityDb` — Identity.API
- `projectDb` — Project.API
- `taskDb` — Tasks.API
- `fileDb` — File.API

Services exchange events via RabbitMQ.

## Tech Stack

- .NET 10 / ASP.NET Core (Minimal API)
- Entity Framework Core + SQL Server
- MassTransit + RabbitMQ
- ASP.NET Core Identity + JWT (stored in an HttpOnly cookie)
- Azure Blob Storage (via `Azure.Storage.Blobs`)
- MailKit/MimeKit — sending email
- .NET Aspire — local orchestration of services and infrastructure
- xUnit + Moq — tests
