using System.Collections.Generic;
using System.Linq;
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
    public class ChangeTagsAction : TicketActionBase
    {
        public IEnumerable<string> Tags { get; set; }

        public ChangeTagsAction()
        {
        }

        public ChangeTagsAction(params string[] tags)
        {
            Tags = tags;
        }
    }
}
