﻿using System;

namespace RpsGame.Commands
{
    public interface ICommand
    {
        Guid EntityId { get; }
    }
}