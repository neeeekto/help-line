using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Actions
{
    public class ChangePriorityAction : TicketActionBase
    {
        public TicketPriority Priority { get; set; }

        public ChangePriorityAction()
        {
        }

        public ChangePriorityAction(TicketPriority priority)
        {
            Priority = priority;
        }
    }
}
