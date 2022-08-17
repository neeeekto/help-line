using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Application.Events;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using MediatR;
using Newtonsoft.Json;

namespace HelpLine.BuildingBlocks.Infrastructure.Outbox
{
    public abstract class OutboxAccessorBase : IOutbox
    {
        private readonly IMongoContext _context;
        private readonly IMediator _mediator;

        private readonly List<OutboxMessage> _queue;

        protected OutboxAccessorBase(IMongoContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
            _queue = new List<OutboxMessage>();
        }

        public async Task Add(OutboxMessage message)
        {
            await _context.GetCollection<OutboxMessage>().InsertOneAsync(_context.Session, message);
            _queue.Add(message);
        }

        public async Task Publish()
        {
            foreach (var message in _queue)
                await Publish(message);
            _queue.Clear();
        }

        protected async Task Publish(OutboxMessage message)
        {
            var collection = _context.GetCollection<OutboxMessage>();
            try
            {
                Type type = GetType(message.Type);
                var request = JsonConvert.DeserializeObject(message.Data, type) as IDomainEventNotification;
                await _mediator.Publish(request);
            }
            catch (Exception e)
            {
                message.Error = e.ToString();
                throw;
            }
            finally
            {
                message.ProcessedDate = DateTime.UtcNow;
                Expression<Func<OutboxMessage, bool>> filter = x => x.Id == message.Id;
                await collection.ReplaceOneAsync(filter, message);
            }
        }

        protected abstract Type GetType(string typeName);
    }
}
