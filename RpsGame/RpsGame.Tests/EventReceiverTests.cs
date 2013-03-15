using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RpsGame.Commands;
using RpsGame.EventStore;
using RpsGame.Events;
using RpsGame.Model;
using RpsGame.Query;

namespace RpsGame.Tests
{
    public class EventReceiverTests
    {
        private DelegatingEventStore _eventStore;
        private ApplicationService _applicationService;
        private HighScoreProjection _highScoreProjection;

        [SetUp]
        public void SetUp()
        {
            _highScoreProjection = new HighScoreProjection();
            _eventStore = new DelegatingEventStore(new InMemoryEventStore(), _highScoreProjection);
            _applicationService = new ApplicationService(_eventStore);
        }


        [Test]
        public void Should_publish_events_to_receivers()
        {
            CreateWinningGame("me", "you");
            CreateWinningGame("me", "you");
            CreateWinningGame("you", "me");
            CreateWinningGame("me", "you");

            Assert.That(_highScoreProjection.HighScores["me"], Is.EqualTo(3));
            Assert.That(_highScoreProjection.HighScores["you"], Is.EqualTo(1));
        }

        private void CreateWinningGame(string createdBy, string opponent)
        {
            var entityId = Guid.NewGuid();
            _applicationService.Handle(new CreateGame(entityId, createdBy, opponent, "lunch", 1));
            _applicationService.Handle(new MakeMove(entityId, createdBy, Move.Paper));
            _applicationService.Handle(new MakeMove(entityId, opponent, Move.Rock));
        }
    }

}