# ProjectMS

Project Management System — мікросервісний backend-застосунок для керування проєктами, задачами, файлами та запрошеннями учасників.

## Архітектура

Застосунок побудований на .NET Aspire і складається з кількох незалежних сервісів, що спілкуються між собою через повідомлення (RabbitMQ / MassTransit).

| Сервіс | Опис |
|---|---|---|
| `Identity.API` | Реєстрація, логін, видача JWT |
| `Project.API` | Створення проєктів, учасники, запрошення |
| `Tasks.API` | Задачі в межах проєкту |
| `File.API` | Завантаження та зберігання файлів задач |
| `Email.Worker` | Фоновий воркер, що надсилає email-запрошення |
| `ProjectMS.AppHost` | Aspire-оркестратор: піднімає інфраструктуру та всі сервіси локально |
| `Shared` | Спільні модулі |

Кожен API-сервіс має власну базу даних (SQL Server):

- `identityDb` — Identity.API
- `projectDb` — Project.API
- `taskDb` — Tasks.API
- `fileDb` — File.API

Сервіси обмінюються подіями через RabbitMQ.

## Технології

- .NET 10 / ASP.NET Core (Minimal API)
- Entity Framework Core + SQL Server
- MassTransit + RabbitMQ
- ASP.NET Core Identity + JWT (зберігається в HttpOnly cookie)
- Azure Blob Storage (через `Azure.Storage.Blobs`)
- MailKit/MimeKit — надсилання email
- .NET Aspire — локальна оркестрація сервісів та інфраструктури
- xUnit + Moq — тести
