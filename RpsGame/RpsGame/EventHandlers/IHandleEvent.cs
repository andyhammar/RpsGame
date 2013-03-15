using RpsGame.Events;

namespace RpsGame.EventHandlers
{
    public interface IHandleEvent<in T> where T : IEvent
    {
        void Handle(T ev);
    }
}