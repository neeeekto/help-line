using System;
using System.Collections.Generic;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels
{
    public class RecipientView
    {
        public string UserId { get; internal set; }
        public string Channel { get; internal set; }
        public IEnumerable<DeliveryStatusView> DeliveryStatuses { get; internal set; }
    }
}
