using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Models;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Channels.Contracts
{
    public interface IChannelSystemRepository : IRepository<ChannelSettings>
    {
        public Task<T> Get<T>(string projectId) where T : ChannelSettings;
    }
}
