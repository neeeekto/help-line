using System;
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
    public class CancelTicketRejectAction : TicketActionBase
    {
        public Guid RejectId { get; }

        public CancelTicketRejectAction()
        {
        }

        public CancelTicketRejectAction(Guid rejectId)
        {
            RejectId = rejectId;
        }
    }
}
