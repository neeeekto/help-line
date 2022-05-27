using System;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SaveTicketFilter
{
    public class SaveTicketFilterCommand : CommandBase<Guid>
    {
        public string ProjectId { get; }
        public TicketSavedFilterData Data { get; }
        public Guid? FilterId { get; }


        public SaveTicketFilterCommand(string projectId, TicketSavedFilterData data, Guid? filterId = null)
        {
            ProjectId = projectId;
            Data = data;
            FilterId = filterId;
        }
    }
}
