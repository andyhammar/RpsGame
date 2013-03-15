using System;

namespace RpsGame.Events
{
    public abstract class GameEvent : IEvent
    {
        protected GameEvent(Guid gameId)
        {
            GameId = gameId;
        }

        public Guid GameId { get; private set; }
    }
}