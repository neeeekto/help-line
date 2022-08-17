using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HelpLine.BuildingBlocks.Domain;

namespace HelpLine.BuildingBlocks.Infrastructure.DomainEventsDispatching
{
    public class DomainEventsAccessorAndCollector : IDomainEventsAccessor, IDomainEventCollector
    {
        private readonly List<Entity> _entities = new List<Entity>();

        public DomainEventsAccessorAndCollector()
        {
        }

        public List<IDomainEvent> GetAllDomainEvents()
        {
            var domainEntities = _entities
                .Where(x => x.DomainEvents != null && x.DomainEvents.Any()).Distinct().ToList();

            return domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();
        }

        public void ClearAllDomainEvents()
        {
            var domainEntities = _entities
                .Where(x => x.DomainEvents != null && x.DomainEvents.Any()).ToList();

            domainEntities
                .ForEach(entity => entity.ClearDomainEvents());

            _entities.Clear();
        }

        public void Add(Entity entity)
        {
            _entities.AddRange(GetEntities(entity));
        }

        private IEnumerable<Entity> GetEntities(Entity rootEntity)
        {
            // TODO: Use cache for reflection, reflection is power, but slow
            List<Entity> entities = new List<Entity>();

            entities.Add(rootEntity);
            var fields = rootEntity.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public).Concat(rootEntity.GetType().BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance| BindingFlags.Public)).ToArray();

            foreach (var field in fields)
            {
                var isEntity = field.FieldType.IsAssignableFrom(typeof(Entity));

                if (isEntity)
                {
                    var entity = field.GetValue(rootEntity) as Entity;
                    entities.Add(entity);
                    entities.AddRange(GetEntities(entity).ToList());
                }

                if (field.FieldType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(field.FieldType))
                {
                    if (field.GetValue(rootEntity) is IEnumerable enumerable)
                    {
                        foreach (var en in enumerable)
                        {
                            if (en is Entity entityItem)
                            {
                                entities.Add(entityItem);
                                entities.AddRange(GetEntities(entityItem));
                            }
                        }
                    }
                }
            }

            return entities;
        }
    }
}
