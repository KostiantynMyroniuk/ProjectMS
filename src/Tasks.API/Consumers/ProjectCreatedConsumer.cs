using MassTransit;
using Shared.Events.Projects;
using Tasks.API.Infrastructure;
using Tasks.API.Models.ProjectSnapshots;

namespace Tasks.API.Consumers
{
    public class ProjectCreatedConsumer : IConsumer<ProjectCreatedEvent>
    {
        private readonly ApplicationDbContext _context;

        public ProjectCreatedConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ProjectCreatedEvent> context)
        {
            var msg = context.Message;

            var project = new ProjectSnapshot
            {
                Name = msg.Name,
                ProjectId = msg.ProjectId,
                IsActive = true
            };

            _context.ProjectSnapshots.Add(project);
            await _context.SaveChangesAsync();
        }
    }
}
