using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.RemoveTicketMessageTemplate
{
    public class RemoveTicketMessageTemplateCommand : CommandBase
    {
        public Guid TemplateId { get; }

        public RemoveTicketMessageTemplateCommand(Guid templateId)
        {
            TemplateId = templateId;
        }


    }
}
