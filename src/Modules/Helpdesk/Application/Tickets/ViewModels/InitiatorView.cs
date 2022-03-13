using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public abstract class InitiatorView
    {
    }

    public class OperatorInitiatorView : InitiatorView
    {
        public Guid OperatorId { get; internal set; }
    }

    public class UserInitiatorView : InitiatorView
    {
        public string UserId { get; internal set; }
    }

    public class SystemInitiatorView : InitiatorView
    {
        public string? Description { get; internal set; }
        public IDictionary<string, string>? Meta { get; internal set; }
    }
}
