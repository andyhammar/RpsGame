using System;
using RpsGame.Model;

namespace RpsGame.Commands
{
    class MakeMove : ICommand
    {
        public MakeMove(Guid entityId, string player, Move move)
        {
            EntityId = entityId;
            Player = player;
            Move = move;
        }

        public Guid EntityId { get; private set; }
        public string Player { get; private set; }
        public Move Move { get; private set; }
    }
}