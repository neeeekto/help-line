﻿using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts
{
    public interface ITicketReopenChecker
    {
        Task<bool> CheckBy(TicketFeedback feedback, ProjectId projectId);
    }
}
