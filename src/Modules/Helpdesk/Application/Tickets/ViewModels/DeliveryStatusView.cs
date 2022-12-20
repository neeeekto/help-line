using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class DeliveryStatusView
    {
        public DateTime Date { get; internal set; }
        public MessageStatus Status { get; internal set; }
        public string? Detail { get; internal set; }
        public IDictionary<string, string>? Meta { get; internal set; }
    }
}
