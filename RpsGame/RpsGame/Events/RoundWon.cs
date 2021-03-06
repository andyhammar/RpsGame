﻿using System;

namespace RpsGame.Events
{
    public class RoundWon : GameEvent
    {
        public RoundWon(Guid gameId, string winner, string loser) : base(gameId)
        {
            Winner = winner;
            Loser = loser;
        }

        public string Winner { get; private set; }
        public string Loser { get; private set; }
    }
}