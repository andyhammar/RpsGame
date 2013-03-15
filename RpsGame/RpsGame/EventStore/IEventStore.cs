using System;
using System.Collections.Generic;
using RpsGame.Events;

namespace RpsGame.EventStore
{
    public interface IEventStore
    {
        IEventStream LoadEventStream(Guid streamId);
        void Append(Guid streamId, long version, IEnumerable<IEvent> events);
    }
}