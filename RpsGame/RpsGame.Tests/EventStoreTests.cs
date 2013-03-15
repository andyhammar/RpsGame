
using System;
using System.Collections.Generic;
using System.Linq;
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
            _eventStore.Append(streamId, 0, new List<IEvent>() { gameCreated });

            var stream = _eventStore.LoadEventStream(streamId);
            Assert.That(stream.Version, Is.EqualTo(1));
            Assert.That(stream, Has.Count.EqualTo(1));
        }

        [Test]
        public void Should_throw_on_append_with_wrong_version()
        {
            var streamId = new Guid();
            var gameCreated = new GameCreated(streamId, "me", "you", "beef", 4);
            _eventStore.Append(streamId, 0, new List<IEvent>() { gameCreated });

            Assert.Throws<ConcurrentAppendException>(() => _eventStore.Append(streamId, 0, new List<IEvent>() { gameCreated }));
        }

        [Test]
        public void should_accept_2nd_append_with_next_version()
        {
            var streamId = new Guid();
            var gameCreated = new GameCreated(streamId, "me", "you", "beef", 4);
            _eventStore.Append(streamId, 0, new List<IEvent>() { gameCreated });

            var stream = _eventStore.LoadEventStream(streamId);
            _eventStore.Append(streamId, 1, new List<IEvent>() { gameCreated });

            stream = _eventStore.LoadEventStream(streamId);

            Assert.That(stream.Version, Is.EqualTo(2));
            Assert.That(stream, Has.Count.EqualTo(2));
        }

        [Test]
        public void Should_be_immutable_event_stream()
        {
            var streamId = new Guid();
            var gameCreated = new GameCreated(streamId, "me", "you", "beef", 4);
            _eventStore.Append(streamId, 0, new[] { gameCreated });

            var stream = _eventStore.LoadEventStream(streamId);
            var before = stream.Count();

            _eventStore.Append(streamId, stream.Version, new List<IEvent>() { gameCreated });
            var after = stream.Count();
            

            Assert.That(before, Is.EqualTo(after));
            Assert.That(_eventStore.LoadEventStream(streamId).Count(), Is.EqualTo(2));
        }
    }
}
