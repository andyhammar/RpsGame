using System;
using RpsGame.Model;

namespace RpsGame.Events
{
    class MoveMade : GameEvent
    {
        public MoveMade(Guid gameId, string player, Move move) : base(gameId)
        {
            Player = player;
            Move = move;
        }

        public string Player { get; private set; }
        public Move Move { get; private set; }
    }
}