using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.AddBan
{
    public class AddBanCommand : CommandBase<Guid>
    {
        public string ProjectId { get; }
        public Ban.Parameters Parameter { get; }
        public string Value { get; }
        public DateTime? ExpiredAt { get; }

        public AddBanCommand(string projectId, Ban.Parameters parameter, string value, DateTime? expiredAt = null)
        {
            ProjectId = projectId;
            Parameter = parameter;
            Value = value;
            ExpiredAt = expiredAt;
        }
    }
}
