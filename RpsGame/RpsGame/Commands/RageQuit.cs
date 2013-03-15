using System;

namespace RpsGame.Commands
{
    class RageQuit : ICommand
    {
        public Guid AggregateId { get; private set; }
        public string Player { get; private set; }

        public RageQuit(Guid gameId, string player)
        {
            AggregateId = gameId;
            Player = player;
        }
    }
}