namespace Project.API.Models.Projects.Dtos
{
    public record ProjectModelDto(Guid Id, string Name, string? Description, string OwnerName, DateTime CreatedAt);

    public record CreateProjectModelDto(string Name, string? Description);
    
    public record UpdateProjectModelDto(string? Name, string? Description);
}
