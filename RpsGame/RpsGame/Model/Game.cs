using System;
using System.Collections.Generic;
using System.Linq;
using RpsGame.CommandHandlers;
using RpsGame.Commands;
using RpsGame.EventHandlers;
using RpsGame.Events;

namespace RpsGame.Model
{
    public class Game : 
        IHandleCommand<CreateGame>, 
        IHandleCommand<MakeMove>, 
        IHandleEvent<GameCreated>, 
        IHandleEvent<MoveMade>, 
        IHandleEvent<RoundWon>, 
        IHandleEvent<GameWon>,
        IHandleEvent<RoundTied>
    {
        public Game()
        {
            _state = GameState.NotStarted;
        }

        public void Handle(GameCreated ev)
        {
            _id = ev.GameId;
            _firstTo = ev.FirstTo;
            _reason = ev.Reason;
            _currentMoves.Add(ev.CreatedBy, null);
            _score.Add(ev.CreatedBy, 0);
            _currentMoves.Add(ev.Opponent, null);
            _score.Add(ev.Opponent, 0);

            _state = GameState.Undecided;
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
            _currentMoves[ev.Player] = ev.Move;
            _state = GameState.WaitingForMove;
        }

        public void Handle(GameWon ev)
        {
            _state = GameState.Over;
        }

        public void Handle(RoundWon ev)
        {
            _score[ev.Winner] = _score[ev.Winner] + 1;
            NewRound();
        }

        public void Handle(RoundTied ev)
        {
            NewRound();
        }

        private void NewRound()
        {
            _state = GameState.Undecided;
            foreach (var key in _currentMoves.Keys.ToList())
            {
                _currentMoves[key] = null;
            }
        }


        public IEnumerable<IEvent> Handle(MakeMove command)
        {
            Validate(command);
            yield return new MoveMade(command.AggregateId, command.Player, command.Move);

            if (_state == GameState.WaitingForMove)
            {
                var roundEvent = GetRoundEvent(command);
                yield return roundEvent;

                var roundWon = roundEvent as RoundWon;

                if (roundWon == null)
                    yield break;
                if(_score[roundWon.Winner] == _firstTo - 1)
                    yield return new GameWon(command.AggregateId,roundWon.Winner,roundWon.Loser);
            }
        }

        private IEvent GetRoundEvent(MakeMove command)
        {
            var otherMove = _currentMoves.Where(x => x.Key != command.Player).FirstOrDefault();
            if (otherMove.Value == command.Move)
                return new RoundTied(command.AggregateId, command.Player, otherMove.Key);
            if (command.Move.IsWinner(otherMove.Value.Value))
                return new RoundWon(command.AggregateId, command.Player, otherMove.Key);
            return new RoundWon(command.AggregateId, otherMove.Key, command.Player);

        }

        private void Validate(MakeMove makeMove)
        {
            AssertState(GameState.Undecided, GameState.WaitingForMove);

            if (!_currentMoves.ContainsKey(makeMove.Player))
                throw new InvalidCommandException(makeMove);

            if (_currentMoves[makeMove.Player].HasValue)
                throw new InvalidCommandException(makeMove);
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
        private int _firstTo;
        private string _reason;
        private readonly Dictionary<string, Move?> _currentMoves = new Dictionary<string, Move?>(2);
        private readonly Dictionary<string, int> _score = new Dictionary<string, int>(2);

    }
}