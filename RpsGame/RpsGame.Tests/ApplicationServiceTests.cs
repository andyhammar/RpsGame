using System;
using System.Linq;
using NUnit.Framework;
using RpsGame.Commands;
using RpsGame.EventStore;
using RpsGame.Events;
using RpsGame.Model;

namespace RpsGame.Tests
{
    public class ApplicationServiceTests
    {
        private InMemoryEventStore _eventStore;
        private ApplicationService _applicationService;

        [SetUp]
        public void BeforeEachTest()
        {
            _eventStore = new InMemoryEventStore();
            _applicationService = new ApplicationService(_eventStore);
        }

        [Test]
        public void Should_win_a_round_after_two_moves()
        {
            var entityId = Guid.NewGuid();
            _applicationService.Handle(new CreateGame(entityId,"me","you","lunch",2));
            _applicationService.Handle(new MakeMove(entityId,"me",Move.Paper));
            _applicationService.Handle(new MakeMove(entityId,"you",Move.Rock));

            var stream = _eventStore.LoadEventStream(entityId);

            var last = stream.Last();
            Assert.That(last,Is.AssignableTo<RoundWon>());
        }        
        
        [Test]
        public void Should_win_a_game_after_all_rounds_won()
        {
            var entityId = Guid.NewGuid();
            _applicationService.Handle(new CreateGame(entityId,"me","you","lunch",2));
            _applicationService.Handle(new MakeMove(entityId,"me",Move.Paper));
            _applicationService.Handle(new MakeMove(entityId,"you",Move.Rock));
            _applicationService.Handle(new MakeMove(entityId,"me",Move.Paper));
            _applicationService.Handle(new MakeMove(entityId, "you", Move.Rock));

            var stream = _eventStore.LoadEventStream(entityId);

            var last = stream.Last();
            Assert.That(last,Is.AssignableTo<GameWon>());
        }

        [Test]
        public void Should_tie_a_round_when_moved_equal()
        {
            var entityId = Guid.NewGuid();
            _applicationService.Handle(new CreateGame(entityId, "me", "you", "lunch", 2));
            _applicationService.Handle(new MakeMove(entityId, "me", Move.Rock));
            _applicationService.Handle(new MakeMove(entityId, "you", Move.Rock));

            var stream = _eventStore.LoadEventStream(entityId);

            var last = stream.Last();
            Assert.That(last, Is.AssignableTo<RoundTied>());
        }

        [Test]
        public void Should_win_a_game_with_many_rounds()
        {
            var entityId = Guid.NewGuid();
            _applicationService.Handle(new CreateGame(entityId, "me", "you", "lunch", 2));
            _applicationService.Handle(new MakeMove(entityId, "you", Move.Paper));
            _applicationService.Handle(new MakeMove(entityId, "me", Move.Rock));
            _applicationService.Handle(new MakeMove(entityId, "me", Move.Paper));
            _applicationService.Handle(new MakeMove(entityId, "you", Move.Rock));
            _applicationService.Handle(new MakeMove(entityId, "me", Move.Rock));
            _applicationService.Handle(new MakeMove(entityId, "you", Move.Rock));
            _applicationService.Handle(new MakeMove(entityId, "me", Move.Rock));
            _applicationService.Handle(new MakeMove(entityId, "you", Move.Rock));
            _applicationService.Handle(new MakeMove(entityId, "me", Move.Rock));
            _applicationService.Handle(new MakeMove(entityId, "you", Move.Paper));

            var stream = _eventStore.LoadEventStream(entityId);

            var last = stream.Last() as GameWon;
            Assert.That(last, Is.Not.Null.And.Property("Winner").EqualTo("you"));
        }

        [Test]
        public void Should_finish_game_after_rageQuit()
        {
            var entityId = Guid.NewGuid();
            _applicationService.Handle(new CreateGame(entityId, "me", "you", "lunch", 2));
            _applicationService.Handle(new MakeMove(entityId, "you", Move.Paper));
            _applicationService.Handle(new RageQuit(entityId, "me"));

            var stream = _eventStore.LoadEventStream(entityId);

            var last = stream.Last() as GameWon;
            Assert.That(last, Is.Not.Null.And.Property("Winner").EqualTo("you"));
            Assert.That(stream, Has.Some.AssignableFrom<PlayerLeftGame>());
        }
    }
}