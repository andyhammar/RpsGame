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
            // load events
            var stream = _eventStore.LoadEventStream(command.AggregateId);

            //instantiate blank aggregate
            var game = new Game() as dynamic;

            //replay events
            stream.ToList().ForEach(x => game.Handle((dynamic) x));

            //execute command
            var events = game.Handle((dynamic) command);

            //store events
            _eventStore.Append(command.AggregateId, stream.Version, events);
        }
    }
}