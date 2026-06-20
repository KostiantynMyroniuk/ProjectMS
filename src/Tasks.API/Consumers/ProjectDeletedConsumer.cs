using MassTransit;
using Shared.Events.Projects;
using Tasks.API.Infrastructure;

namespace Tasks.API.Consumers
{
    public class ProjectDeletedConsumer : IConsumer<ProjectDeletedEvent>
    {
        private readonly ApplicationDbContext _context;

        public ProjectDeletedConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ProjectDeletedEvent> context)
        {
            var msg = context.Message;

            var project = await _context.ProjectSnapshots.FindAsync(msg.ProjectId);

            _context.ProjectSnapshots.Remove(project!);
            await _context.SaveChangesAsync();
        }
    }
}
