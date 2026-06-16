using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Tasks.API.Models.ProjectTasks;

namespace Tasks.API.Apis
{
    public static class TaskApi
    {
        public static IEndpointRouteBuilder MapTaskApi(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/")
                .WithTags("Tasks")
                .RequireAuthorization();

            group.MapGet("/projects/{projectId:guid}/tasks/{taskId:guid}", GetTaskById);
            group.MapGet("/projects/{projectId:guid}/tasks", GetTasks);
            group.MapPost("/projects/{projectId:guid}/tasks", CreateTask);

            return app;
        }

        public static async Task<IResult> GetTaskById(
            Guid taskId,
            Guid projectId,
            [AsParameters] TaskServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var isMember = await services.Context.ProjectMemberships
                .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            if (!isMember)
            {
                services.Logger.LogWarning("User {UserId} attempted to access project {ProjectId} without membership", userId, projectId);

                return Results.NotFound();
            }

            var task = await services.Context.Tasks.FindAsync(taskId);

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

            var isMember = await services.Context.ProjectMemberships
                .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            if (!isMember)
            {
                services.Logger.LogWarning("User {UserId} attempted to access project {ProjectId} without membership", userId, projectId);

                return Results.NotFound();
            }

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

            var isMember = await services.Context.ProjectMemberships
                .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            if (!isMember)
            {
                services.Logger.LogWarning("User {UserId} attempted to access project {ProjectId} without membership", userId, projectId);

                return Results.NotFound();
            }

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
    }
}
