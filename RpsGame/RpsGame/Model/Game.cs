using System;
using System.Collections.Generic;
using RpsGame.Commands;
using RpsGame.Events;

namespace RpsGame.Model
{
    public class Game
    {
        public void Handle(GameCreated gameCreated)
        {
            Id = gameCreated.GameId;
            Player1 = gameCreated.CreatedBy;
            Player2 = gameCreated.Opponent;
            FirstTo = gameCreated.FirstTo;
            Reason = gameCreated.Reason;
        }

        public IEnumerable<IEvent> Handle(CreateGame createGame)
        {
            return new[] { new GameCreated(createGame.AggregateId, createGame.CreatedBy, createGame.Opponent, createGame.Reason, createGame.FirstTo) };
        }

        private Guid Id { get; set; }
        private string Player1 { get; set; }
        private string Player2 { get; set; }
        private int FirstTo { get; set; }
        private string Reason { get; set; }
    }
}