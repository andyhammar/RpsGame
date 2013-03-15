using System;

namespace RpsGame.Commands
{
    class RageQuit : ICommand
    {
        public Guid EntityId { get; private set; }
        public string Player { get; private set; }

        public RageQuit(Guid gameId, string player)
        {
            EntityId = gameId;
            Player = player;
        }
    }
}