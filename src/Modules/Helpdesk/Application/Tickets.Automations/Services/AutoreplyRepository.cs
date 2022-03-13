using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Models;
using HelpLine.Modules.Helpdesk.Domain.Tickets;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;
using Tag = HelpLine.Modules.Helpdesk.Domain.Tickets.State.Tag;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Automations.Services
{
    public class AutoreplyRepository : IAutoreplyRepository
    {
        private readonly IMongoContext _ctx;
        private readonly IMapper _mapper;

        public AutoreplyRepository(IMongoContext ctx, IMapper mapper)
        {
            _ctx = ctx;
            _mapper = mapper;
        }

        public async Task<Autoreply?> Get(TicketCreatedEvent evt)
        {
            // TODO: Yes, we got all scenarios, i thinks their must be less 0.01GB
            var scenarios = await GetScenarios(evt);
            var foundScenario = scenarios.FirstOrDefault(x => CheckScenario(x, evt));
            if (foundScenario == null)
                return null;
            if (!foundScenario.Action.Message.TryGetValue(evt.Language.Value, out var messageDto))
                return null;
            if (foundScenario.Action.Resolve)
                return new Autoreply(foundScenario.Id,
                    _mapper.Map<Message>(messageDto),
                    foundScenario.Action.Tags.Select(x => new Tag(x)),
                    foundScenario.Action.Resolve);
            return new Autoreply(foundScenario.Id,
                _mapper.Map<Message>(messageDto),
                foundScenario.Action.Tags.Select(x => new Tag(x)),
                foundScenario.Action.Reminder != null
                    ? _mapper.Map<TicketReminder>(foundScenario.Action.Reminder)
                    : null);
        }

        private async Task<IEnumerable<AutoreplyScenario>> GetScenarios(TicketCreatedEvent evt)
        {
            return await _ctx.GetCollection<AutoreplyScenario>()
                .Find(x => x.Enabled && x.ProjectId == evt.ProjectId.Value)
                .SortBy(x => x.Weight)
                .ToListAsync();
        }

        private bool CheckScenario(AutoreplyScenario scenario, TicketCreatedEvent evt)
        {
            var result = false;
            var condition = scenario.Condition;
            if (condition.Languages.Any())
                result = condition.Languages.Contains(evt.Language.Value);
            if (result && condition.Attachments.HasValue)
                result = (evt.Message?.Attachments?.Any() ?? false) == condition.Attachments;
            if (result && condition.TagConditions.Any())
            {
                var tags = evt.Tags.Select(x => x.Value);
                result = condition.TagConditions.All(x => x.Check(tags));
            }

            if (result && condition.Keywords?.Any() == true)
            {
                condition.Keywords.TryGetValue(evt.Language.Value, out var query);
                if (!string.IsNullOrEmpty(query))
                    result = CheckTextByKeywords(evt.Message?.Text ?? "", query);
            }

            return result;
        }

        private bool CheckTextByKeywords(string message, string condition)
        {
            message = message.ToLower();
            condition = condition.Replace("INCLUDE", "").Replace("EXCLUDE", "!").Replace("NOT", "!");

            var pattern = "\".*?\"";
            var exp = Regex.Replace(condition, pattern, m => $"Message.Contains({m})");

            var p = Expression.Parameter(typeof(string), "Message");

            LambdaExpression e;
            try
            {
                e = DynamicExpressionParser.ParseLambda(new[] {p}, null, exp);
            }
            catch (ParseException)
            {
                return false;
            }


            bool.TryParse(e.Compile().DynamicInvoke(message)?.ToString(), out var result);
            return result;
        }
    }
}
