using System;
using RpsGame.Commands;

namespace RpsGame.Model
{
    internal class InvalidCommandException : Exception
    {
        public InvalidCommandException(ICommand failedCommand)
        {
            FailedCommand = failedCommand;
        }

        public InvalidCommandException()
        {
            throw new NotImplementedException();
        }

        public ICommand FailedCommand { get; private set; }
    }
}