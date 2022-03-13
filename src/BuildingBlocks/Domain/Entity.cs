using System.Collections.Generic;
using System.Threading.Tasks;

namespace HelpLine.BuildingBlocks.Domain
{
    public abstract class Entity
    {
        private List<IDomainEvent> _domainEvents;

        /// <summary>
        /// Domain events occurred.
        /// </summary>
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        /// <summary>
        /// Add domain event.
        /// </summary>
        /// <param name="domainEvent"></param>
        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<IDomainEvent>();
            _domainEvents.Add(domainEvent);
        }

        public virtual void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected void CheckRule(IBusinessRule rule)
        {
            if (rule.IsBroken())
            {
                throw new BusinessRuleValidationException(rule);
            }
        }

        protected  async Task CheckRule(IBusinessRuleAsync asyncRule)
        {
            if (await asyncRule.IsBroken())
            {
                throw new BusinessRuleValidationException(asyncRule);
            }
        }
    }
}
