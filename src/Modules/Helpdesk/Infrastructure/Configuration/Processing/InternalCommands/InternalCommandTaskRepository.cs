using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.InternalCommands;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Infrastructure.Configuration.Processing.InternalCommands
{
    internal class InternalCommandTaskRepository : IInternalCommandTaskRepository
    {
        private readonly IMongoContext _context;

        public InternalCommandTaskRepository(IMongoContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(InternalCommandTaskBase task)
        {
            var collection = _context.GetCollection<InternalCommandTask>();
            await collection.ReplaceOneAsync(x => x.Id == task.Id, (InternalCommandTask)task, new ReplaceOptions {IsUpsert = true});
        }
    }
}
