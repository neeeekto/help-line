using System;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.SharedKernel;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux;
using HelpLine.Modules.Helpdesk.Application.Tickets.Aux.Models;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels;
using HelpLine.Modules.Helpdesk.Domain.Projects;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Contracts;
using HelpLine.Modules.Helpdesk.Domain.Tickets.Events;
using HelpLine.Modules.Helpdesk.Domain.Tickets.State;
using MongoDB.Driver;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Services
{
    public class TicketChecker : ITicketChecker
    {
        internal static string? GetIp(UserMeta meta) =>
            meta.FirstOrDefault(x => x.Key.ToUpperInvariant() == "IP").Value;

        private readonly IMongoContext _context;

        public TicketChecker(IMongoContext context)
        {
            _context = context;
        }

        public async Task<bool> ProjectIsExist(ProjectId projectId)
        {
            var collection = _context.GetCollection<Project>();
            var exist = await collection.Find(x => x.Id == projectId).AnyAsync();
            return exist;
        }

        public async Task<bool> LanguageIsExist(ProjectId projectId, LanguageCode languageCode)
        {
            var project = await _context.GetCollection<Project>().Find(x => x.Id == projectId).FirstOrDefaultAsync();
            return project.Languages.Contains(languageCode);
        }

        public async Task<bool> CheckBan(TicketCreatedEvent evt)
        {
            var bans = await _context.GetCollection<Ban>().Find(x =>
                    x.ProjectId == evt.ProjectId.Value && (x.ExpiredAt == null || x.ExpiredAt < DateTime.UtcNow))
                .ToListAsync();
            foreach (var ban in bans)
            {
                switch (ban.Parameter)
                {
                    case Ban.Parameters.Text
                        when evt.Message?.Text?.ToUpperInvariant().Contains(ban.Value.ToUpperInvariant()) == true:
                        return true;
                    case Ban.Parameters.Ip:
                    {
                        var ip = GetIp(evt.UserMeta);
                        if (!string.IsNullOrEmpty(ip) && !string.IsNullOrEmpty(ip) && ban.Value == ip)
                            return true;
                        break;
                    }
                }
            }

            return false;
        }
    }
}
