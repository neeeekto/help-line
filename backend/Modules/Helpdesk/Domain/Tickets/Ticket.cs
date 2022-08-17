using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Commands;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Rules;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Domain.Tickets
{
    public class Ticket : EventsSourcingAggregateBase<TicketId, TicketState>
    {


        #region Constructors and impl methods

        private Ticket(TicketId id, TicketState state, int version) : base(id, state, version)
        {
        }

        private Ticket(TicketId id) : base(id, new TicketState(), InitVersin)
        {
        }


        protected override void ApplyToState(EventBase<TicketId> evt)
        {
            State.ApplyEvent(evt);
        }

        #endregion

        public static async Task<Ticket> Create(
            ITicketIdFactory idFactory,
            ITicketServicesProvider services,
            ProjectId projectId,
            LanguageCode language,
            Initiator initiator,
            IEnumerable<Tag> tags,
            UserChannels userChannels,
            UserMeta userMeta,
            TicketMeta meta,
            Message? message
        )
        {
            var ticketId = await idFactory.GetNext(projectId);
            var ticket = new Ticket(ticketId);

            ticket.CheckRule(new InitiatorMustExistRule(initiator));
            await ticket.CheckRule(new ProjectMustExistRule(services.Checker, projectId));
            var isUserInitiator = initiator is UserInitiator;
            if (isUserInitiator)
                ticket.CheckRule(new MessageCannotBeEmptyRule(message, true));

            var status = isUserInitiator
                ? TicketStatus.Opened(TicketStatusType.New)
                : TicketStatus.Pending(TicketStatusType.Answered);
            var evt = new TicketCreatedEvent(ticketId, initiator, projectId, language, tags, userChannels,
                userMeta, message, status,
                TicketPriority.Normal, meta);
            await ticket.CheckRule(new BanRule(services.Checker, evt));
            ticket.RiseEvent(evt);
            await ticket.Execute(new ExecuteCreationHookTicketCommand(evt), services, evt.Initiator);
            return ticket;
        }

        public async Task<TResult> Execute<TResult>(TicketCommand<TResult> command, ITicketServicesProvider services, Initiator initiator)
        {
            var ctx = new CommandContext(this, RiseEvent, services, CheckRule, CheckRule, initiator);
            return await command.Execute(ctx);
        }
    }
}
