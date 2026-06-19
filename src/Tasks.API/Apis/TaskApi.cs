using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Tasks.API.Infrastructure;
using Tasks.API.Models.ProjectTasks;

namespace Tasks.API.Apis
{
    public static class TaskApi
    {
        public static IEndpointRouteBuilder MapTaskApi(this IEndpointRouteBuilder app)
        {
            var tasksGroup = app.MapGroup("/projects/{projectId:guid}")
                .WithTags("Tasks")
                .RequireAuthorization();

            tasksGroup.MapGet("tasks/{taskId:guid}", GetTaskById);
            tasksGroup.MapGet("tasks", GetTasks);
            tasksGroup.MapPost("tasks", CreateTask);
            tasksGroup.MapPut("tasks/{taskId:guid}", UpdateTask);
            tasksGroup.MapDelete("tasks/{taskId:guid}", DeleteTask);

            return app;
        }

        public static async Task<IResult> GetTaskById(
            Guid taskId,
            Guid projectId,
            [AsParameters] TaskServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var accessError = await CheckAccessAsync(services.Context, projectId, userId);

            if (accessError is not null)
                return accessError;

            var task = await services.Context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

            if (task == null)
                return Results.NotFound();

            return Results.Ok(task);
        }

        public static async Task<IResult> GetTasks(
            Guid projectId,
            [AsParameters] TaskServices services,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var userId = services.IdentityService.GetUserId();

            var accessError = await CheckAccessAsync(services.Context, projectId, userId);

            if (accessError is not null)
                return accessError;

            var query = services.Context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId);

            var totalCount = await query.CountAsync();

            var tasks = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginatedResult = new PaginatedList<ProjectTask>(
                tasks, pageNumber, pageSize, totalCount);
            
            return Results.Ok(paginatedResult);
        }

        public static async Task<IResult> CreateTask(
            Guid projectId,
            ProjectTaskDto taskDto,
            [AsParameters] TaskServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var accessError = await CheckAccessAsync(services.Context, projectId, userId);

            if (accessError is not null)
                return accessError;

            var task = new ProjectTask(taskDto.Title)
            {
                Description = taskDto.Description,
                Priority = taskDto.Priority,
                Status = taskDto.Status,

                ProjectId = projectId
            };

            services.Context.Tasks.Add(task);
            await services.Context.SaveChangesAsync();

            services.Logger.LogInformation("User {UserId} created task {TaskId} in project {ProjectId}", userId, task.Id, projectId);

            return Results.Created($"/projects/{projectId}/tasks/{task.Id}", task);
        }

        public static async Task<IResult> UpdateTask(
            Guid taskId,
            Guid projectId,
            ProjectTaskDto taskDto,
            [AsParameters] TaskServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var accessError = await CheckAccessAsync(services.Context, projectId, userId);

            if (accessError is not null)
                return accessError;

            var task = await services.Context.Tasks
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ProjectId == projectId);

            if (task == null)
                return Results.NotFound();

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.Status = taskDto.Status;
            task.Priority = taskDto.Priority;

            await services.Context.SaveChangesAsync();

            services.Logger.LogInformation("User {UserId} updated task {TaskId} in project {ProjectId}", userId, task.Id, projectId);

            return Results.Ok(task);
        }

        public static async Task<IResult> DeleteTask(
            Guid taskId,
            Guid projectId,
            [AsParameters] TaskServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var accessError = await CheckAccessAsync(services.Context, projectId, userId);

            if (accessError is not null)
                return accessError;

            var task = await services.Context.Tasks
                .FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId);

            services.Context.Tasks.Remove(task);
            await services.Context.SaveChangesAsync();

            return Results.NoContent();
        }

        private static async Task<IResult?> CheckAccessAsync(
            ApplicationDbContext context, Guid projectId, string userId)
        {
            var projectExists = await context.ProjectSnapshots
                .AnyAsync(ps => ps.ProjectId == projectId && ps.IsActive);

            if (!projectExists)
                return Results.NotFound();

            var isMember = await context.ProjectMemberships
                .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            return isMember ? null : Results.Forbid();
        }
    }
}
