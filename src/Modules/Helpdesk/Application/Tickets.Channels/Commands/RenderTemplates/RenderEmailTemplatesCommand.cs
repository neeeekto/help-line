using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Contracts;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.DTO;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.RenderTemplates
{
    public abstract class RenderEmailTemplateCommandBase : CommandBase<EmailRendererResult>
    {
        public TicketView Ticket { get; }
        public EmailChannelSettings Settings { get; }

        protected RenderEmailTemplateCommandBase(TicketView ticket, EmailChannelSettings settings)
        {
            Ticket = ticket;
            Settings = settings;
        }
    }

    public class RenderMessageEmailTemplateCommand : RenderEmailTemplateCommandBase
    {
        public RenderMessageEmailTemplateCommand(TicketView ticket, EmailChannelSettings settings) : base(ticket, settings)
        {
        }
    }

    public class RenderFeedbackEmailTemplateCommand : RenderEmailTemplateCommandBase
    {
        public Guid FeedbackId { get; }

        public RenderFeedbackEmailTemplateCommand(TicketView ticket, EmailChannelSettings settings, Guid feedbackId) : base(ticket, settings)
        {
            FeedbackId = feedbackId;
        }
    }
}
