using System.Collections.Generic;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.BuildingBlocks.Domain.EventsSourcing;

namespace HelpLine.Modules.Helpdesk.Application.Configuration.Projections
{
    public interface IProjector
    {
        Task Project(IEnumerable<IDomainEvent> events);
    }
}
