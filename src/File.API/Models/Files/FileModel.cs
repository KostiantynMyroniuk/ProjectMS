namespace File.API.Models.Files
{
    public class FileModel
    {
        public Guid Id { get; set; }
        public string OriginalName { get; set; } = default!;
        public string Name { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public Guid TaskId { get; set; }
        public string UserId { get; set; } = default!;

        public FileModel(string originalName)
        {
            Id = Guid.CreateVersion7();
            OriginalName = originalName;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
