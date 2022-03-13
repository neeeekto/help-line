using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Commands.SaveChannelSetting
{
    class SaveChannelSettingCommandHandler : ICommandHandler<SaveChannelSettingCommand>
    {
        private readonly IMongoContext _context;

        public SaveChannelSettingCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SaveChannelSettingCommand request, CancellationToken cancellationToken)
        {
            await _context.GetCollection<ChannelSettings>().ReplaceOneAsync(
                x => x.Key == request.Settings.Key && x.ProjectId == request.Settings.ProjectId, request.Settings,
                new ReplaceOptions()
                {
                    IsUpsert = true
                });
            return Unit.Value;
        }
    }
}
