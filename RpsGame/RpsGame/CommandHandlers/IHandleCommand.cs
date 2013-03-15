using System.Collections.Generic;
using RpsGame.Commands;
using RpsGame.Events;

namespace RpsGame.CommandHandlers
{
    public interface IHandleCommand <in T> where T : ICommand
    {
        IEnumerable<IEvent> Handle(T command);
    }
}