using System.Collections.Generic;
using RpsGame.Events;

namespace RpsGame.EventStore
{
    public interface IEventStream : IEnumerable<IEvent>
    {
        long Version { get; }
    }
}