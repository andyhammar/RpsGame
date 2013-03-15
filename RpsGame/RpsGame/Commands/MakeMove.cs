    using System;
using RpsGame.Model;

namespace RpsGame.Commands
{
    public class MakeMove : ICommand
    {
        public MakeMove(Guid entityId, string player, Move move)
        {
            AggregateId = entityId;
            Player = player;
            Move = move;
        }

        public Guid AggregateId { get; private set; }
        public string Player { get; private set; }
        public Move Move { get; private set; }
    }
}