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
        IHandleCommand<RageQuit>,
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
            _firstTo = ev.FirstTo;
            _score.Add(ev.CreatedBy, 0);
            _score.Add(ev.Opponent, 0);

            _state = GameState.Undecided;
        }

        public IEnumerable<IEvent> Handle(CreateGame createGame)
        {
            Validate(createGame);
            return new[]
                       {
                           new GameCreated(
                               createGame.AggregateId,
                               createGame.CreatedBy,
                               createGame.Opponent,
                               createGame.Reason,
                               createGame.FirstTo)
                       };
        }

        public void Handle(MoveMade ev)
        {
            _lastMove = Tuple.Create(ev.Player, ev.Move);
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
            _lastMove = null;
        }


        public IEnumerable<IEvent> Handle(MakeMove command)
        {
            Validate(command);
            yield return new MoveMade(command.AggregateId, command.Player, command.Move);

            if (_state != GameState.WaitingForMove) yield break;

            var roundEvent = GetRoundEvent(command);
            yield return roundEvent;

            var roundWon = roundEvent as RoundWon;

            if (roundWon == null)
                yield break;
            if (_score[roundWon.Winner] == _firstTo - 1)
                yield return new GameWon(command.AggregateId, roundWon.Winner, roundWon.Loser);
        }

        private IEvent GetRoundEvent(MakeMove command)
        {
            if (_lastMove.Item2 == command.Move)
                return new RoundTied(command.AggregateId, command.Player, _lastMove.Item1);
            if (command.Move.IsWinner(_lastMove.Item2))
                return new RoundWon(command.AggregateId, command.Player, _lastMove.Item1);
            return new RoundWon(command.AggregateId, _lastMove.Item1, command.Player);
        }

        private void Validate(MakeMove makeMove)
        {
            AssertState(GameState.Undecided, GameState.WaitingForMove);

            if (!_score.ContainsKey(makeMove.Player))
                throw new InvalidCommandException(makeMove);

            if (_lastMove != null && _lastMove.Item1 == makeMove.Player)
                throw new InvalidCommandException(makeMove);
        }

        public IEnumerable<IEvent> Handle(RageQuit command)
        {
            AssertState(GameState.WaitingForMove, GameState.Undecided, GameState.NotStarted);
            var otherMove = _score.FirstOrDefault(x => x.Key != command.Player);
            var opponent = otherMove.Key;
            yield return new PlayerLeftGame(command.AggregateId, command.Player, opponent);
            yield return new GameWon(command.AggregateId, opponent, command.Player);
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

        private GameState _state;
        private int _firstTo;
        private Tuple<string, Move> _lastMove;
        private readonly Dictionary<string, int> _score = new Dictionary<string, int>(2);
    }
}