using RpsGame.Commands;

namespace RpsGame
{
    public interface IApplicationService 
    {
        void Handle(ICommand command);
    }
}