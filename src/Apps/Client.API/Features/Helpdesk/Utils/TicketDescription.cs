using System.Collections.Generic;
using HelpLine.BuildingBlocks.Application.TypeDescription;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;

namespace HelpLine.Apps.Client.API.Features.Helpdesk.Utils
{
    internal class TicketDescription : Description
    {
        public TicketDescription() : base(new DescriptionClassMap[]
        {
            new TicketDescriptionMap(),
            new FeedbackReviewDescriptionMap(),
            new TicketDiscussionStateDescriptionMap(),
            new UserIdInfonMap(),
            new TicketEventViewMap(),
            new InitiatorViewMap(),
            new OperatorInitiatorViewMap(),
            new UserInitiatorViewMap(),
            new SystemInitiatorViewMap(),
        }, typeof(TicketView))
        {
        }

        private class FeedbackReviewDescriptionMap : DescriptionClassMap<FeedbackReviewView>
        {
            public override void Init()
            {
                MapField(x => x.Message);
                MapField(x => x.Score);
                MapField(x => x.Solved);
                MapField(x => x.DateTime);
            }
        }

        private class TicketDiscussionStateDescriptionMap : DescriptionClassMap<TicketDiscussionStateView>
        {
            public override void Init()
            {
                MapField(x => x.IterationCount);
                MapField(x => x.LastMessageType);
                MapField(x => x.LastReplyDate);
            }
        }

        private class UserIdInfonMap : DescriptionClassMap<UserIdInfoView>
        {
            public override void Init()
            {
                MapField(x => x.Channel);
                MapField(x => x.Type);
                MapField(x => x.UserId);
                MapField(x => x.UseForDiscussion);
            }
        }

        private class TicketEventViewMap : DescriptionClassMap<TicketEventView>
        {
            public override void Init()
            {
                MapField(x => x.Initiator);
                MapField(x => x.CreateDate);
            }
        }

        private class InitiatorViewMap : DescriptionClassMap<InitiatorView>
        {
            public override void Init()
            {
                SetChildren(typeof(OperatorInitiatorView),
                    typeof(UserInitiatorView),
                    typeof(SystemInitiatorView));
            }
        }

        private class OperatorInitiatorViewMap : DescriptionClassMap<OperatorInitiatorView>
        {
            public override void Init()
            {
                MapField(x => x.OperatorId);
            }
        }

        private class UserInitiatorViewMap : DescriptionClassMap<UserInitiatorView>
        {
            public override void Init()
            {
                MapField(x => x.UserId);
            }
        }

        private class SystemInitiatorViewMap : DescriptionClassMap<SystemInitiatorView>
        {
            public override void Init()
            {
            }
        }

        private class TicketDescriptionMap : DescriptionClassMap<TicketView>
        {
            public override void Init()
            {
                MapField(x => x.Tags);
                MapField(x => x.Language);
                MapField(x => x.Priority);
                MapField(x => x.Status.Type);
                MapField(x => x.HardAssigment);
                MapField(x => x.AssignedTo);
                MapField(x => x.CreateDate);
                MapField(x => x.DateOfLastStatusChange);
                MapField(x => x.DiscussionState);
                MapField(x => x.DiscussionState);
                MapField(x => x.Title);
                MapField(x => x.Feedbacks);
                MapField(x => x.UserIds);
                MapField(x => x.UserMeta);
                MapField(x => x.Events);
            }
        }
    }
}
