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
        public void Should_win_game_after_two_moves()
        {
            var entityId = Guid.NewGuid();
            _applicationService.Handle(new CreateGame(entityId,"me","you","lunch",1));
            _applicationService.Handle(new MakeMove(entityId,"me",Move.Paper));
            _applicationService.Handle(new MakeMove(entityId,"you",Move.Rock));

            var stream = _eventStore.LoadEventStream(entityId);

            var last = stream.Last();
            Assert.That(last,Is.AssignableTo<GameWon>());
        }
    }
}