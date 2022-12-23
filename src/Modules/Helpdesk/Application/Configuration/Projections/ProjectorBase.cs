using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;

namespace HelpLine.Modules.Helpdesk.Application.Configuration.Projections
{
    internal abstract class ProjectorBase<TId>
    {
        protected static Task When(EventBase<TId> evt)
        {
            return Task.CompletedTask;
        }
    }
}
