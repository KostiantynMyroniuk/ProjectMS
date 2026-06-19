using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Events.Projects
{
    public record ProjectDeletedEvent(Guid ProjectId);
}
