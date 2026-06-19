using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared.Events.ProjectMembers;
using Tasks.API.Infrastructure;
using Tasks.API.Models.ProjectMemberhips;

namespace Tasks.API.Consumers
{
    public class UserAddedToProjectConsumer : IConsumer<UserAddedToProjectEvent>
    {
        private readonly ApplicationDbContext _context;

        public UserAddedToProjectConsumer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<UserAddedToProjectEvent> context)
        {
            var msg = context.Message;

            var exists = await _context.ProjectMemberships
                .AnyAsync(pm => pm.ProjectId == msg.ProjectId && 
                                pm.UserId == msg.UserId);

            if (!exists)
            {
                var projectMembership = new ProjectMembership
                {
                    ProjectId = msg.ProjectId,
                    UserId = msg.UserId
                };

                _context.ProjectMemberships.Add(projectMembership);

                await _context.SaveChangesAsync();
            }
        }
    }
}
