using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Events
{
    public record UserAddedToProjectEvent(string UserId, Guid ProjectId);
}
