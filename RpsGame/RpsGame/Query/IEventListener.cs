using System.Collections.Generic;
using RpsGame.Events;

namespace RpsGame.Query
{
    public interface IEventListener
    {

        void Receive(IEnumerable<IEvent> events);

    }
}