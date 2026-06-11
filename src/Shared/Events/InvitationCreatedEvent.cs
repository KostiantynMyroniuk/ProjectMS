using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Events
{
    public record InvitationCreatedEvent(
        string Email, 
        string ProjectName, 
        string InvitedByName, 
        string AcceptUrl, 
        DateTime ExpiresAt);
    
}
