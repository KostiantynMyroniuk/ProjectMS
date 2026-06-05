using Microsoft.EntityFrameworkCore;
using Project.API.Models;
using Project.API.Models.Dtos;
using Shared.Models;

namespace Project.API.Apis
{
    public static class ProjectApi
    {
        public static IEndpointRouteBuilder MapProjectApi(this IEndpointRouteBuilder app)
        {
            var project = app.MapGroup("/").RequireAuthorization();

            project.MapGet("/projects/{projectId:guid}", GetProject);
            project.MapGet("/my-projects", GetMyProjects);
            project.MapPost("/projects", CreateProject);
            project.MapPatch("/projects/{projectId:guid}", UpdateProject);
            project.MapDelete("/projects/{projectId:guid}", DeleteProject);

            return app;
        }

        public static async Task<IResult> GetMyProjects(
            [AsParameters] ProjectServices services,
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber < 1)
                return Results.BadRequest("Invalid page number");

            if (pageSize <= 0)
                return Results.BadRequest("Invalid page size");

            var userId = services.IdentityService.GetUserId();

            var validProjectsQuery = services.Context.Projects
                .AsNoTracking()
                .Where(p => p.OwnerId == userId);

            var totalCount = await validProjectsQuery
                .CountAsync();

            var paginatedProjects = await validProjectsQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var paginatedList = new PaginatedList<ProjectModel>(paginatedProjects, pageNumber, pageSize, totalCount);

            return Results.Ok(paginatedList);
        }

        public static async Task<IResult> GetProject(
            Guid projectId,
            [AsParameters] ProjectServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var project = await services.Context.Projects.FindAsync(projectId);

            if (project is null)
            {
                services.Logger.LogWarning("Project {ProjectId} not found", projectId);
                return Results.NotFound();
            }

            if (userId != project.OwnerId)
            {
                services.Logger.LogWarning("User {UserId} attempted to access to project {ProjectId}. Access denied.", userId, projectId);
                return Results.NotFound();
            }

            return Results.Ok(project);
        }

        public static async Task<IResult> CreateProject(
            ProjectModelDto projectDto,
            [AsParameters] ProjectServices services)
        {
            var userId = services.IdentityService.GetUserId();
            var userName = services.IdentityService.GetUserName();

            var project = new ProjectModel(projectDto.Name)
            {
                Description = projectDto.Description,
                OwnerId = userId,
                OwnerName = userName
            };

            services.Context.Projects.Add(project);
            await services.Context.SaveChangesAsync();

            services.Logger.LogInformation("Project {ProjectId} created by user {UserId}", project.Id, userId);

            return Results.Created($"/projects/{project.Id}", project);
        }

        public static async Task<IResult> UpdateProject(
            Guid projectId,
            UpdateProjectModelDto projectDto,
            [AsParameters] ProjectServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var project = await services.Context.Projects.FindAsync(projectId);

            if (project is null)
            {
                services.Logger.LogWarning("Project {ProjectId} not found", projectId);
                return Results.NotFound();
            }

            if (project.OwnerId != userId)
            {
                services.Logger.LogWarning("User {UserId} attempted to access to project {ProjectId}. Access denied.", userId, projectId);
                return Results.NotFound();
            }

            project.Update(
                projectDto.Name ?? project.Name,
                projectDto.Description ?? project.Description);

            await services.Context.SaveChangesAsync();

            services.Logger.LogInformation("User {UserId} updated project {ProjectId}", userId, projectId);

            return Results.Ok(project);
        }

        public static async Task<IResult> DeleteProject(
            Guid projectId,
            [AsParameters] ProjectServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var project = await services.Context.Projects.FindAsync(projectId);

            if (project is null)
            {
                services.Logger.LogWarning("Project {ProjectId} not found", projectId);
                return Results.NotFound();
            }

            if (project.OwnerId != userId)
            {
                services.Logger.LogWarning("User {UserId} attempted to access to project {ProjectId}. Access denied.", userId, projectId);
                return Results.NotFound();
            }

            services.Context.Projects.Remove(project);
            await services.Context.SaveChangesAsync();

            services.Logger.LogInformation("User {UserId} deleted project {ProjectId}", userId, projectId);

            return Results.NoContent();
        }
    }
}
