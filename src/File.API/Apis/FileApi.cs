using File.API.Infrastructure;
using File.API.Models.Files;
using Microsoft.EntityFrameworkCore;

namespace File.API.Apis
{
    public static class FileApi
    {
        public static IEndpointRouteBuilder MapFileApi(this IEndpointRouteBuilder app)
        {
            var fileGroup = app.MapGroup("/projects/{projectId:guid}/tasks/{taskId:guid}/files")
                .WithTags("Files")
                .RequireAuthorization();

            fileGroup.MapPost("/", UploadFile).DisableAntiforgery();


            return app;
        }

        public static async Task<IResult> UploadFile(
            Guid projectId,
            Guid taskId,
            IFormFile file,
            [AsParameters] FileServices services)
        {
            var userId = services.IdentityService.GetUserId();

            var hasAccess = await CheckAccessAsync(services.Context, projectId, userId);

            if (hasAccess is not null)
            {
                return hasAccess;
            }

            try
            {
                var uploadResult = await services.StorageService.UploadFileAsync(file);

                services.Logger.LogInformation("File {FileName} uploaded to storage by user {UserId}", uploadResult, userId);

                var fileModel = new FileModel(file.FileName)
                {
                    Name = uploadResult,

                    TaskId = taskId,
                    UserId = userId,
                };

                services.Context.Files.Add(fileModel);
                await services.Context.SaveChangesAsync();

                services.Logger.LogInformation("User {UserId} successfully uploaded file {FileId}", userId, fileModel.Id);
            }
            catch (Exception ex)
            {
                services.Logger.LogError(ex, "Error in file upload occurs");
                return Results.InternalServerError();
            }

            return Results.Ok();
        }

        private static async Task<IResult?> CheckAccessAsync(
            ApplicationDbContext context, Guid projectId, string userId)
        {
            var projectExists = await context.Projects
                .AnyAsync(ps => ps.ProjectId == projectId && ps.IsActive);

            if (!projectExists)
                return Results.NotFound();

            var isMember = await context.ProjectMemberships
                .AnyAsync(pm => pm.ProjectId == projectId && pm.UserId == userId);

            return isMember ? null : Results.NotFound();
        }
    }
}
