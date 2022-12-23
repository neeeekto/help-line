using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.SetBanSetting
{
    internal class SetBanSettingCommandHandler : ICommandHandler<SetBanSettingCommand>
    {
        private readonly IMongoContext _context;

        public SetBanSettingCommandHandler(IMongoContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(SetBanSettingCommand request, CancellationToken cancellationToken)
        {
            request.Settings.ProjectId = request.ProjectId;
            await _context.GetCollection<BanSettings>().ReplaceOneAsync(
                x => x.ProjectId == request.Settings.ProjectId, request.Settings, new ReplaceOptions
                {
                    IsUpsert = true
                });
            return Unit.Value;
        }
    }
}
