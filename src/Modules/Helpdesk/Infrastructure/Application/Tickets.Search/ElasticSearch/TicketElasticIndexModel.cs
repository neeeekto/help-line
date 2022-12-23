using System;
using System.Collections.Generic;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using Nest;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.ElasticSearch
{
    [ElasticsearchType(Name = "TicketView")]
	public class TicketElasticIndexModel
	{
		[Keyword(Store = true)]
		public string Id { get; set; }
		
		[Keyword]
		public string ProjectId { get; set; }
		
		[Keyword]
		public IReadOnlyList<string> Tags { get; set; }
		
		[Keyword]
		public string Language { get; set; }

        [Number(NumberType.Integer)]
        public TicketPriority Priority { get; set; }
        
        [Number(NumberType.Integer)]
        public TicketStatusKind StatusKind { get; set; }
        
        [Number(NumberType.Integer)]
        public TicketStatusType StatusType { get; set; }
        
        [Boolean]
        public bool HardAssigment { get; set; }

        [Keyword(NullValue = "")]
        public string AssignedTo { get; set; }
        
        [Date]
        public DateTime CreateDate { get; set; }
        
        [Nested]
        public TicketDiscussionStateIndexModel DiscussionState { get; set; }
        
        [Nested]
        public IReadOnlyList<FeedbackReviewIndexModel> Feedbacks { get; set; }
        
        [Nested]
        public IReadOnlyList<UserIdInfoIndexModel> UserIds { get; set; }
        
        [Nested]
        public IReadOnlyList<UserMetaRecord> UserMeta { get; set; }
        
        [Nested]
        public TicketMetaIndexModel Meta { get; set; }

        // ---
        


        #region nested types

        public class TicketDiscussionStateIndexModel
        {
	        [Date]
	        public DateTime? LastReplyDate { get; set; }
	        
	        [Number(NumberType.Integer)]
	        public TicketDiscussionStateView.MessageType LastMessageType { get; internal set; }
	        
	        [Number(NumberType.Integer)]
	        public int IterationCount { get; internal set; }
	        
	        [Boolean]
	        public bool HasAttachments { get; internal set; }
        }
        
        public class UserIdInfoIndexModel
		{
			[Keyword]
			public string UserId { get;  set; }
			[Keyword]
			public string Channel { get;  set; }
			[Number(NumberType.Integer)]
			public UserIdType Type { get;  set; }
			[Boolean]
			public bool UseForDiscussion { get; internal set; }
		}
        
        public class UserMetaRecord
        {
	        [Keyword]
	        public string Key { get; set; }

	        [Keyword]
	        public string Value { get; set; }
        }

        public class FeedbackReviewIndexModel
        {
	        [Date]
	        public DateTime DateTime { get;  set; }
	        
	        [Number(NumberType.Integer)]
	        public int Score { get;  set; }
	        
	        public IDictionary<string, int> OptionalScores { get; internal set; }
	        
            [Boolean]
            public bool? Solved { get; set; }

            [Boolean]
            public bool? HasMessage { get; set; }


            [Nested]
            public IReadOnlyList<OptionalCriteria> Criterias { get; set; }

            public class OptionalCriteria
            {
                [Keyword]
                public string Key { get; set; }

                [Number(NumberType.Integer)]
                public int Value { get; set; }
            }
        }
        
        public class TicketMetaIndexModel
        {
	        [Keyword]
	        public string? FromTicket { get; set; }
	        
	        [Keyword]
	        public string Source { get; set; }
	        
	        [Keyword]
	        public string? Platform { get; set; }
        }
        
        public abstract class TicketEventIndexModel
        {
	        [Keyword]
	        public Guid Id { get; internal set; }
	        public InitiatorView Initiator { get; internal set; }
	        public DateTime CreateDate { get; internal set; }
        }

        #endregion
    }
}
