using System;

namespace RpsGame.Events
{
    public class PlayerLeftGame : GameEvent
    {
        public PlayerLeftGame(Guid gameId, string player, string opponent) : base(gameId)
        {
            Player = player;
            Opponent = opponent;
        }

        public string Player { get; private set; }
        public string Opponent { get; private set; }
    }
}