using System.Collections.Generic;
using RpsGame.Events;

namespace RpsGame.EventStore
{
    class EventStream : List<IEvent>, IEventStream
    {
        public EventStream(IEnumerable<IEvent> collection, long version) : base(collection)
        {
            Version = version;
        }

        public long Version { get; private set; }
    }
}