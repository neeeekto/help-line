using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HelpLine.Modules.Helpdesk.Application.Tickets.DTO;
using HelpLine.Modules.Helpdesk.Domain.Operators;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Profiles
{
    internal class TicketDtoProfiles : Profile
    {
        public TicketDtoProfiles()
        {
            CreateMap<MessageDto, Message>()
                .ConstructUsing((x, ctx) =>
                {
                    return new Message(x.Text, x.Attachments?.ToList());
                }).ForAllMembers(x => x.AllowNull());
            CreateMap<TicketFeedbackDto, TicketFeedback>().ConstructUsing(x =>
                new TicketFeedback(x.Score, x.Message, x.Solved, x.OptionalScores));
            CreateMap<InitiatorDto, Initiator>().ConstructUsing((initiator, ctx) =>
            {
                switch (initiator)
                {
                    case SystemInitiatorDto systemInitiator:
                        return new SystemInitiator(systemInitiator.Description,
                            systemInitiator.Meta?.ToDictionary(x => x.Key, x => x.Value));

                    case OperatorInitiatorDto operatorInitiator:
                        return new OperatorInitiator(new OperatorId(operatorInitiator.OperatorId));

                    case UserInitiatorDto userInitiator:
                        return new UserInitiator(new UserId(userInitiator.UserId));
                    default:
                        throw new ApplicationException($"[{initiator.GetType().FullName}]: Unknown initiator type");
                }
            });
            CreateMap<TicketReminderDto, TicketReminder>().ConstructUsing((x, ctx) =>
            {
                switch (x)
                {
                    case TicketFinalReminderDto finalReminderDto:
                        return new TicketFinalReminder(finalReminderDto.Delay,
                            ctx.Mapper.Map<Message>(finalReminderDto.Message), finalReminderDto.Resolve);
                    case TicketSequentialReminderDto sequentialReminderDto:
                        return new TicketSequentialReminder(sequentialReminderDto.Delay,
                            ctx.Mapper.Map<Message>(sequentialReminderDto.Message),
                            ctx.Mapper.Map<TicketReminder>(sequentialReminderDto.Next));
                    default:
                        throw new ApplicationException($"[{x.GetType().FullName}]: Unknown reminder type");
                }
            });
        }
    }
}
