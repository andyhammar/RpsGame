using System.Linq;
using RpsGame.Commands;
using RpsGame.EventStore;
using RpsGame.Model;

namespace RpsGame
{
    public class ApplicationService : IApplicationService
    {
        private readonly IEventStore _eventStore;

        public ApplicationService(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public void Handle(ICommand command)
        {
            // TODO: load events
            var stream = _eventStore.LoadEventStream(command.AggregateId);

            // TODO: instantiate blank aggregate
            var game = new Game() as dynamic;

            // TODO: replay events
            stream.ToList().ForEach(x => game.Handle((dynamic) x));

            // TODO: execute command
            var events = game.Handle((dynamic) command);

            // TODO: store events
            _eventStore.Append(command.AggregateId, stream.Version, events);
        }
    }
}

// MAGIC STUFF

//var aggregate = CreateNewAggregate() as dynamic;

//var events = aggregate.Handle((dynamic) command);}