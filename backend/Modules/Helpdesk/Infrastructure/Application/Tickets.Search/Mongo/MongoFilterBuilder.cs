using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters;
using HelpLine.Modules.Helpdesk.Application.Tickets.Search.Filters.Values;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels.Events;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Application.Tickets.Search.Mongo;

internal class MongoFilterBuilder
{
    private readonly FilterContext _context;
    private readonly FilterDefinitionBuilder<TicketView> _filterBuilder;

    public MongoFilterBuilder(FilterContext context)
    {
        _context = context;
        _filterBuilder = new FilterDefinitionBuilder<TicketView>();
    }

    public FilterDefinition<TicketView> Build(TicketFilterBase? filter)
    {
        return filter switch
        {
            TicketFilterGroup groupFilter => Build(groupFilter),
            TicketAssigmentFilter assigmentFilter => Build(assigmentFilter),
            TicketAttachmentFilter attachmentFilter => Build(attachmentFilter),
            TicketCreateDateFilter createDateFilter => Build(createDateFilter),
            TicketEventExistFilter ticketEventExistFilter => Build(ticketEventExistFilter),
            TicketFeedbackFilter ticketFeedbackFilter => Build(ticketFeedbackFilter),
            TicketHardAssigmentFilter ticketFeedbackFilter => Build(ticketFeedbackFilter),
            TicketIdFilter ticketIdFilter => Build(ticketIdFilter),
            TicketIterationCountFilter ticketIterationCount => Build(ticketIterationCount),
            TicketLanguageFilter ticketLanguageFilter => Build(ticketLanguageFilter),
            TicketLastMessageTypeFilter lastMessageTypeFilter => Build(lastMessageTypeFilter),
            TicketLastReplyFilter lastReplyFilter => Build(lastReplyFilter),
            TicketMetaFilter ticketMetaFilter => Build(ticketMetaFilter),
            TicketProjectFilter ticketProjectFilter => Build(ticketProjectFilter),
            TicketStatusFilter ticketProjectFilter => Build(ticketProjectFilter),
            TicketTagsFilter ticketTagsFilter => Build(ticketTagsFilter),
            TicketUserIdFilter ticketTagsFilter => Build(ticketTagsFilter),
            TicketUserMetaFilter ticketTagsFilter => Build(ticketTagsFilter),
            _ => _filterBuilder.Empty
        };
    }


    private FilterDefinition<TicketView> Build(TicketFilterGroup filter)
    {
        var filters = filter.Filters.Select(Build).ToArray();
        return filter.Intersection ? _filterBuilder.And(filters) : _filterBuilder.Or(filters);
    }

    private FilterDefinition<TicketView> Build(TicketAssigmentFilter filter)
    {
        return _filterBuilder.Or(filter.Values.Select(x =>
        {
            return x switch
            {
                TicketAssigmentFilter.CurrentOperator => _filterBuilder.Where(x =>
                    x.AssignedTo == _context.CurrentOperator),
                TicketAssigmentFilter.Unassigned => _filterBuilder.Where(x => x.AssignedTo == null),
                TicketAssigmentFilter.Operator oper => _filterBuilder.Where(x => x.AssignedTo == oper.Id),
                _ => _filterBuilder.Empty
            };
        }));
    }

    private FilterDefinition<TicketView> Build(TicketAttachmentFilter filter)
    {
        return _filterBuilder.Eq(x => x.DiscussionState.HasAttachments, filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketFeedbackFilter filter)
    {
        var fb = new FilterDefinitionBuilder<FeedbackReviewView>();
        var result = fb.Empty;
        if (filter.Date is not null)
        {
            result &= Build<FeedbackReviewView>(x => x.DateTime, filter.Date);
        }

        if (filter.Scores.Any())
        {
            result &= fb.In(x => x.Score, filter.Scores);
        }

        if (filter.Solved is not null)
        {
            result &= fb.Eq(x => x.Solved, filter.Solved);
        }

        if (filter.OptionalScores.Any())
        {
            foreach (var score in filter.OptionalScores)
            {
                result &= score.Value.Any()
                    ? fb.In(t => t.OptionalScores[score.Key], score.Value)
                    : fb.Exists(t => t.OptionalScores[score.Key], false);
            }
        }

        if (filter.HasMessage is not null)
        {
            result &= (bool)filter.HasMessage
                ? fb.Where(x => x.Message != null && x.Message != "")
                : fb.Where(x => x.Message != null && x.Message == "");
        }

        return _filterBuilder.ElemMatch(x => x.Feedbacks, result);
    }

    private FilterDefinition<TicketView> Build(TicketHardAssigmentFilter filter)
    {
        return _filterBuilder.Eq(x => x.HardAssigment, filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketIdFilter filter)
    {
        return _filterBuilder.In(x => x.Id, filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketIterationCountFilter filter)
    {
        return Build<TicketView, int>(x => x.DiscussionState.IterationCount, filter.Operator, filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketLanguageFilter filter)
    {
        return _filterBuilder.In(x => x.Language, filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketLastMessageTypeFilter filter)
    {
        return _filterBuilder.Eq(x => x.DiscussionState.LastMessageType, filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketLastReplyFilter filter)
    {
        return Build<TicketView>(x => x.DiscussionState.LastReplyDate, filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketProjectFilter filter)
    {
        return _filterBuilder.In(x => x.ProjectId, filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketStatusFilter filter)
    {
        var typeFilter = _filterBuilder.In(x => x.Status.Type, filter.Type);
        if (filter.Kind is not null)
        {
            return _filterBuilder.And(_filterBuilder.Eq(x => x.Status.Kind, filter.Kind), typeFilter);
        }

        return typeFilter;
    }

    private FilterDefinition<TicketView> Build(TicketTagsFilter filter)
    {
        return filter.Exclude
            ? _filterBuilder.AnyNin(x => x.Tags, filter.Tags)
            : _filterBuilder.AnyIn(x => x.Tags, filter.Tags);
    }

    private FilterDefinition<TicketView> Build(TicketUserIdFilter filter)
    {
        var fb = new FilterDefinitionBuilder<UserIdInfoView>();
        var result = fb.In(x => x.UserId, filter.Ids);
        if (filter.Channel is not null)
        {
            result &= fb.In(x => x.Channel, filter.Channel);
        }

        if (filter.Type is not null)
        {
            result &= fb.Eq(x => x.Type, filter.Type);
        }

        if (filter.UseForDiscussion is not null)
        {
            result &= fb.Eq(x => x.UseForDiscussion, filter.UseForDiscussion);
        }

        return _filterBuilder.ElemMatch(x => x.UserIds, result);
    }

    private FilterDefinition<TicketView> Build(TicketUserMetaFilter filter)
    {
        return _filterBuilder.In(x => x.UserMeta[filter.Key], filter.Value);
    }

    private FilterDefinition<TicketView> Build(TicketMetaFilter filter)
    {
        var result = new List<FilterDefinition<TicketView>>();
        if (filter.Platforms?.Any() == true)
        {
            result.Add(_filterBuilder.In(x => x.Meta.Platform, filter.Platforms));
        }

        if (filter.Sources?.Any() == true)
        {
            result.Add(_filterBuilder.In(x => x.Meta.Source, filter.Sources));
        }

        if (filter.FromTickets?.Any() == true)
        {
            result.Add(_filterBuilder.In(x => x.Meta.FromTicket, filter.FromTickets));
        }

        return _filterBuilder.And(result);
    }

    private FilterDefinition<TicketView> Build(TicketEventExistFilter filter)
    {
        var fb = new FilterDefinitionBuilder<TicketEventView>();
        return Build(filter, fb.In("_t", filter.Types));
    }

    private FilterDefinition<TicketView> Build(TicketEventFilterBase filter, FilterDefinition<TicketEventView> extend)
    {
        var fb = new FilterDefinitionBuilder<TicketEventView>();
        var result = extend;
        if (filter.CreateDate is not null)
        {
            result &= Build<TicketEventView>(x => x.CreateDate, filter.CreateDate);
        }

        if (filter.Initiators is not null)
        {
            foreach (var fi in filter.Initiators)
            {
                switch (fi)
                {
                    case TicketOperatorInitiatorFilterValue ov:
                        result |= fb.Where(x => x.Initiator is OperatorInitiatorView &&
                                                ov.Ids.Contains(((x.Initiator as OperatorInitiatorView)!).OperatorId));
                        break;
                    default:
                        continue;
                }
            }
        }


        return _filterBuilder.ElemMatch(x => x.Events, result);
    }

    private FilterDefinition<TicketView> Build(TicketCreateDateFilter filter)
    {
        return Build<TicketView>(x => x.CreateDate, filter.Value);
    }

    private FilterDefinition<TSrc> Build<TSrc>(Expression<Func<TSrc, DateTime?>> field,
        FilterDateValueBase filterDateValue)
    {
        var fb = new FilterDefinitionBuilder<TSrc>();
        switch (filterDateValue)
        {
            case FilterDateGroup gr:
                var filters = gr.Values.Select(x => Build(field, x)).ToArray();
                return gr.Intersection ? fb.And(filters) : fb.Or(filters);
            case FilterDateValue dv:
                return Build(field, dv);
            default:
                return fb.Empty;
        }
    }

    private FilterDefinition<TSrc> Build<TSrc, TField>(Expression<Func<TSrc, TField>> field,
        TicketFilterOperators @operator, TField value)
    {
        var fb = new FilterDefinitionBuilder<TSrc>();

        return @operator switch
        {
            TicketFilterOperators.Equal => fb.Eq(field, value),
            TicketFilterOperators.NotEqual => fb.Ne(field, value),
            TicketFilterOperators.Less => fb.Lt(field, value),
            TicketFilterOperators.LessOrEqual => fb.Lte(field, value),
            TicketFilterOperators.Great => fb.Gt(field, value),
            TicketFilterOperators.GreatOrEqual => fb.Gte(field, value),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private FilterDefinition<TSrc> Build<TSrc>(Expression<Func<TSrc, DateTime?>> field,
        FilterDateValue filterDateValue)
    {
        var fb = new FilterDefinitionBuilder<TSrc>();
        var value = filterDateValue.DateTime ?? DateTime.UtcNow;
        if (filterDateValue.Action != null)
        {
            var op = filterDateValue.Action.Operation == FilterDateValueAction.Operations.Add ? 1 : -1;
            value = value.Add(op * filterDateValue.Action.Amount);
        }

        return filterDateValue.Operator switch
        {
            TicketFilterOperators.Equal => fb.Eq(field, value),
            TicketFilterOperators.NotEqual => fb.Ne(field, value),
            TicketFilterOperators.Less => fb.Lt(field, value),
            TicketFilterOperators.LessOrEqual => fb.Lte(field, value),
            TicketFilterOperators.Great => fb.Gt(field, value),
            TicketFilterOperators.GreatOrEqual => fb.Gte(field, value),
            _ => throw new ArgumentOutOfRangeException("filterDateValue.Operator",
                "Unknown filterDateValue operator type")
        };
    }
}
