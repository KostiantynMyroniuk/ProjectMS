using File.API.Infrastructure;
using File.API.Models.Projects;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.Projects;

namespace File.API.Consumers
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

            var alreadyExist = await _context.Projects.AnyAsync(p => p.ProjectId == msg.ProjectId);

            if (!alreadyExist)
            {
                var project = new ProjectSnapshot
                {
                    ProjectId = msg.ProjectId,
                    Name = msg.Name,
                    IsActive = true
                };

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}
