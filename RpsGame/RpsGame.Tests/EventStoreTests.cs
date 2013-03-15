
using System;
using System.Collections.Generic;
using NUnit.Framework;
using RpsGame.EventStore;
using RpsGame.Events;

namespace RpsGame.Tests
{
    
    public class EventStoreTests
    {
        private InMemoryEventStore _eventStore;

        [SetUp]
        public void BeforeEachTest()
        {
            _eventStore = new InMemoryEventStore();
        }

        [Test]
        public void Should_store_events()
        {
            var streamId = new Guid();
            var gameCreated = new GameCreated(streamId, "me", "you", "beef", 4);
            _eventStore.Append(streamId,0, new List<IEvent>() {gameCreated});

            var stream = _eventStore.LoadEventStream(streamId);
            Assert.That(stream.Version,Is.EqualTo(1));
            Assert.That(stream,Has.Count.EqualTo(1));
        }

        [Test]
        public void Should_throw_on_append_with_wrong_version()
        {
            var streamId = new Guid();
            var gameCreated = new GameCreated(streamId, "me", "you", "beef", 4);
            _eventStore.Append(streamId, 0, new List<IEvent>() { gameCreated });

            Assert.Throws<ConcurrentAppendException>(() => _eventStore.Append(streamId, 0, new List<IEvent>() { gameCreated }));
        }
    }
}
