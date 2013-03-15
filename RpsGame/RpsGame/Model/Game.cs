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
            _id = gameCreated.GameId;
            _player1 = gameCreated.CreatedBy;
            _player2 = gameCreated.Opponent;
            _firstTo = gameCreated.FirstTo;
            _reason = gameCreated.Reason;
        }

        public IEnumerable<IEvent> Handle(CreateGame createGame)
        {
            Validate(createGame);
            return new[] { new GameCreated(
                createGame.AggregateId, 
                createGame.CreatedBy, 
                createGame.Opponent, 
                createGame.Reason, 
                createGame.FirstTo) };
        }

        private static void Validate(CreateGame createGame)
        {
            if (createGame.AggregateId == Guid.Empty
                || !createGame.FirstTo.IsValid()
                || !createGame.CreatedBy.IsValid()
                || !createGame.Opponent.IsValid()
                || !createGame.Reason.IsValid()
                )
                throw new InvalidCommandException(createGame);
        }

        private Guid _id;
        private string _player1;
        private string _player2;
        private int _firstTo;
        private string _reason;
    }
}