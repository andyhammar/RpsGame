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

        public ICommand FailedCommand { get; private set; }
    }
}