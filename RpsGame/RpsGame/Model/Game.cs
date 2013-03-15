using System;
using System.Collections.Generic;
using System.Linq;
using RpsGame.CommandHandlers;
using RpsGame.Commands;
using RpsGame.EventHandlers;
using RpsGame.Events;

namespace RpsGame.Model
{
    public class Game : IHandleEvent<GameCreated>, IHandleEvent<MoveMade>, IHandleCommand<CreateGame>, IHandleCommand<MakeMove>
    {
        public Game()
        {
            _state = GameState.NotStarted;
        }

        public void Handle(GameCreated ev)
        {
            _id = ev.GameId;
            _player1 = ev.CreatedBy;
            _player2 = ev.Opponent;
            _firstTo = ev.FirstTo;
            _reason = ev.Reason;
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

        public void Handle(MoveMade ev)
        {
            //AndreasHammar [2013-03-15 12:32]: todo
            _moves[ev.Player] = ev.Move;
        }

        public IEnumerable<IEvent> Handle(MakeMove command)
        {
            Validate(command);
            throw new NotImplementedException();
        }

        private void Validate(MakeMove makeMove)
        {
            AssertState(GameState.GameUndecided, GameState.GameWaitingForMove);
            if (_moves.ContainsKey(makeMove.Player))
        }

        private void Validate(CreateGame createGame)
        {
            AssertState(GameState.NotStarted);
        }

        private void AssertState(params GameState[] states)
        {
            if (!states.Any(s => s == _state))
                throw new InvalidCommandException();
        }

        private Guid _id;
        private GameState _state;
        //todo - remove and only use dict
        private string _player1;
        private string _player2;
        private int _firstTo;
        private string _reason;
        private Dictionary<string, Move?> _moves = new Dictionary<string, Move?>(2);
    }
}