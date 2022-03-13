using System;
using System.Linq.Expressions;
using MongoDB.Driver;
using HelpLine.BuildingBlocks.Application.Outbox;
using HelpLine.BuildingBlocks.Infrastructure.Data;

namespace HelpLine.BuildingBlocks.Infrastructure.Outbox
{
    public class OutboxMessageRepository : RepositoryBase<OutboxMessage>
    {
        public OutboxMessageRepository(IMongoContext context) : base(context)
        {
        }


        protected override Expression<Func<OutboxMessage, bool>> GetIdFilter(OutboxMessage entity)
        {
            return x => x.Id == entity.Id;
        }
    }
}