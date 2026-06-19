using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Events.ProjectMembers
{
    public record UserRemovedFromProjectEvent(string UserId, Guid ProjectId);
}
