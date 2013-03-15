using System;
using System.Collections.Generic;
using System.Linq;
using RpsGame.Events;
using RpsGame.Query;

namespace RpsGame.EventStore
{
    public class DelegatingEventStore : IEventStore
    {
        private readonly IEventStore _eventStore;
        private readonly IEventListener _projection;

        public DelegatingEventStore(IEventStore eventStore, IEventListener projection)
        {
            _eventStore = eventStore;
            _projection = projection;
        }

        public IEventStream LoadEventStream(Guid streamId)
        {
            return _eventStore.LoadEventStream(streamId);
        }

        public void Append(Guid streamId, long version, IEnumerable<IEvent> events)
        {
            _eventStore.Append(streamId,version,events);
            _projection.Receive(events.ToList());
        }
    }
}