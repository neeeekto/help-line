using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.ChangeTicketMessageTemplateOrder
{
    public class ChangeTicketMessageTemplateOrderCommand : CommandBase
    {
        public Guid TemplateId { get; }
        public int NewOrder { get; }

        public ChangeTicketMessageTemplateOrderCommand(Guid templateId, int newOrder)
        {
            TemplateId = templateId;
            NewOrder = newOrder;
        }


    }
}
