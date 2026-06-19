using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Events.ProjectMembers
{
    public record UserAddedToProjectEvent(string UserId, Guid ProjectId);
}
