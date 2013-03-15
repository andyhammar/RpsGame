using System;
using System.Collections.Generic;
using RpsGame.CommandHandlers;
using RpsGame.Commands;
using RpsGame.EventHandlers;
using RpsGame.Events;

namespace RpsGame.Model
{
    public class Game : IHandleEvent<GameCreated>, IHandleEvent<MoveMade>, IHandleCommand<CreateGame>
    {
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

        private static void Validate(CreateGame createGame)
        {
        }

        private Guid _id;
        private string _player1;
        private string _player2;
        private int _firstTo;
        private string _reason;
        private Dictionary<string, Move?> _moves = new Dictionary<string, Move?>(2);
    }
}