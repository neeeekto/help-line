using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.UpdateTicketMessageTemplate
{
    public class UpdateTicketMessageTemplateCommand : CommandBase
    {
        public Guid TemplateId { get; }
        public Dictionary<string, TicketMessageTemplateContent?> Contents { get; }
        public string ProjectId { get; }
        public string? Group { get; }

        public UpdateTicketMessageTemplateCommand(string projectId, Guid templateId,
            Dictionary<string, TicketMessageTemplateContent?> contents,
            string? @group)
        {
            Contents = contents;
            ProjectId = projectId;
            Group = @group;
            TemplateId = templateId;
        }
    }
}
