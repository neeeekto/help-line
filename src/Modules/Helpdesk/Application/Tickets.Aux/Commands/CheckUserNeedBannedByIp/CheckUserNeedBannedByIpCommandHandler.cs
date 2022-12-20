using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Configuration.Commands;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.ViewModels;
using MediatR;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Commands.CheckUserNeedBannedByIp
{
    internal class CheckUserNeedBannedByIpCommandHandler : ICommandHandler<CheckUserNeedBannedByIpCommand>
    {
        private readonly IMongoContext _context;
        private readonly IRepository<Ban> _repository;

        public CheckUserNeedBannedByIpCommandHandler(IMongoContext context, IRepository<Ban> repository)
        {
            _context = context;
            _repository = repository;
        }

        public async Task<Unit> Handle(CheckUserNeedBannedByIpCommand request, CancellationToken cancellationToken)
        {
            var settings = await _context.GetCollection<BanSettings>().Find(x => x.ProjectId == request.ProjectId)
                .FirstOrDefaultAsync(cancellationToken);
            // Find only by IP! No userID
            var ticketsCount = await _context.GetCollection<TicketView>()
                .Find(x => x.UserMeta.Any(x => x.Value == request.Ip) && x.CreateDate >=
                    (DateTime.UtcNow - settings.Interval)).CountDocumentsAsync(cancellationToken);

            if (ticketsCount >= settings.TicketsCount)
            {
                var currentBan =
                    await _repository.FindOne(x => x.Parameter == Ban.Parameters.Ip && x.Value == request.Ip &&
                                                        (x.ExpiredAt == null || x.ExpiredAt > DateTime.UtcNow));
                if (currentBan is null)
                    await _repository.Add(new Ban
                    {
                        Id = Guid.NewGuid(),
                        Parameter = Ban.Parameters.Ip,
                        Value = request.Ip,
                        ProjectId = request.ProjectId,
                        ExpiredAt = DateTime.UtcNow + settings.BanDelay
                    });
            }

            return Unit.Value;
        }
    }
}
