namespace Project.API.Models.Dtos
{
    public record ProjectModelDto(string Name, string? Description);
    
    public record UpdateProjectModelDto(string? Name, string? Description);
}
