using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Events.Projects
{
    public record ProjectCreatedEvent(Guid ProjectId, string Name, string OwnerId);
}
