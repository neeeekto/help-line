using System;

namespace HelpLine.BuildingBlocks.Bus
{
    public class SubscriptionHandler<THandler>
    {
        public SubscriptionHandler(THandler handler, string eventName, Type eventType)
        {
            Handler = handler;
            EventName = eventName;
            EventType = eventType;
        }

        public THandler Handler { get; }

        public string EventName { get; }
        public Type EventType { get; }
    }
}
