using System;
using HelpLine.BuildingBlocks.Application.Search.Contracts;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search
{
    internal class TicketValueMapper : IValueMapper
    {
        public bool TryMap(Type target, object value, out object result)
        {
            result = value;
            return false;
        }
    }
}
