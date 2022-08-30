using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Domain;
using HelpLine.Modules.Helpdesk.Domain.Tickets;

namespace HelpLine.Modules.Helpdesk.Domain.Operators
{
    public class Operator : Entity, IAggregateRoot
    {
        public OperatorId Id { get; private set; }
        public IEnumerable<TicketId> Favorite { get; private set; }
        public ReadOnlyDictionary<string, IEnumerable<Guid>> Roles { get; private set; }

        public static async Task<Operator> Create(OperatorId operatorId)
        {
            return new Operator(operatorId);
        }

        private Operator(OperatorId operatorId)
        {
            Id = operatorId;
            Favorite = new TicketId[] { };
            Roles = new ReadOnlyDictionary<string, IEnumerable<Guid>>(new Dictionary<string, IEnumerable<Guid>>());
        }

        public void SetRole(string projectId, IEnumerable<Guid> roles)
        {
            var current = Roles.ToDictionary(x => x.Key,
                x => x.Value);
            if (current.ContainsKey(projectId))
                current[projectId] = roles;
            else
                current.Add(projectId, roles);

            Roles = new ReadOnlyDictionary<string, IEnumerable<Guid>>(current);
        }

        public void AddToFavorite(TicketId ticketId)
        {
            Favorite = Favorite.Concat(new[] {ticketId}).Distinct();
        }

        public void RemoveFromFavorite(TicketId ticketId)
        {
            Favorite = Favorite.Where(x => x != ticketId);
        }
    }
}
