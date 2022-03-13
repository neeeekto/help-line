using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CreateTicketMessageTemplate
{
    public class CreateTicketMessageTemplateCommand : CommandBase<Guid>
    {
        public Dictionary<string, TicketMessageTemplateContent?> Contents { get; }
        public string ProjectId { get; }
        public string? Group { get; }

        public CreateTicketMessageTemplateCommand(string projectId,
            Dictionary<string, TicketMessageTemplateContent?> contents,
            string? @group = null)
        {
            Contents = contents;
            ProjectId = projectId;
            Group = @group;
        }
    }
}
