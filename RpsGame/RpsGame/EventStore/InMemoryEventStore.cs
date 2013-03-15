using System;
using System.Collections.Generic;
using RpsGame.Events;

namespace RpsGame.EventStore
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<Guid, Tuple<long, List<IEvent>>> _streams = new Dictionary<Guid, Tuple<long, List<IEvent>>>();
        private readonly object _lock = new object();

        public IEventStream LoadEventStream(Guid streamId)
        {
            var ensuredStream = GetEnsuredStream(streamId, 0);
            return new EventStream(ensuredStream.Item2, ensuredStream.Item1);
        }

        public void Append(Guid streamId, long version, IEnumerable<IEvent> events)
        {
            Tuple<long,List<IEvent>> eventStream;
            lock (_lock)
            {
                eventStream = GetEnsuredStream(streamId, version);
                if(version != eventStream.Item1)
                {
                    throw new ConcurrentAppendException();
                }
            }
            eventStream.Item2.AddRange(events);
            _streams[streamId] = Tuple.Create(eventStream.Item1 + 1, eventStream.Item2);
        }

        private Tuple<long, List<IEvent>> GetEnsuredStream(Guid streamId, long version)
        {
            return _streams.ContainsKey(streamId) ? _streams[streamId]: Tuple.Create(version, new List<IEvent>());
        }
    }
}