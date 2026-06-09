using Microsoft.EntityFrameworkCore;
using Project.API.Models.Projects;
using Project.API.Models.Projects.Dtos;
using Shared.Models;

namespace Project.API.Apis.Projects
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
                .Where(p => p.OwnerId == userId || p.ProjectMembers.Any(pm => pm.UserId == userId));

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

            var project = await services.Context.Projects
                .Include(p => p.ProjectMembers)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project is null)
            {
                services.Logger.LogWarning("Project {ProjectId} not found", projectId);
                return Results.NotFound();
            }

            var hasAccess = project.OwnerId == userId || project.ProjectMembers.Any(pm => pm.UserId == userId);

            if (!hasAccess)
            {
                services.Logger.LogWarning("User {UserId} tried to access project {ProjectId} without permission", userId, projectId);
                return Results.NotFound();
            }

            var projectDto = new ProjectModelDto(
                project.Id,
                project.Name,
                project.Description,
                project.OwnerName,
                project.CreatedAt
            );

            return Results.Ok(projectDto);
        }

        public static async Task<IResult> CreateProject(
            CreateProjectModelDto projectDto,
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

            var project = await services.Context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

            if (project is null)
            {
                services.Logger.LogWarning("Project {ProjectId} not found", projectId);
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

            var project = await services.Context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.OwnerId == userId);

            if (project is null)
            {
                services.Logger.LogWarning("Project {ProjectId} not found", projectId);
                return Results.NotFound();
            }

            services.Context.Projects.Remove(project);
            await services.Context.SaveChangesAsync();

            services.Logger.LogInformation("User {UserId} deleted project {ProjectId}", userId, projectId);

            return Results.NoContent();
        }
    }
}
