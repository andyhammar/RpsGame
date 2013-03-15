using System;

namespace RpsGame.Events
{
    public class RoundTied : GameEvent
    {
        public RoundTied(Guid gameId, string player1, string player2)
            : base(gameId)
        {
            Player1 = player1;
            Player2 = player2;
        }

        public string Player1 { get; private set; }
        public string Player2 { get; private set; }
    }
}