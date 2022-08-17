using System;
using MediatR;

namespace HelpLine.Modules.Helpdesk.Application.Tickets.Macros.Triggers
{
    public abstract class ScenarioTriggerBase
    {
        public static string GetEventName(INotification evt) => evt.GetType().FullName!;
        public static string GetEventName<T>() => typeof(T).FullName!;

        public string Event { get; }

        protected ScenarioTriggerBase(string @event)
        {
            Event = @event;
        }
    }

    public abstract class ScenarioTriggerBase<TEvent> : ScenarioTriggerBase where TEvent : INotification
    {
        protected ScenarioTriggerBase() : base(GetEventName<TEvent>())
        {
        }
    }
}
