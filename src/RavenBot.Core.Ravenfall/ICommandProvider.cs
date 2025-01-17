﻿using RavenBot.Core.Handlers;
using RavenBot.Core.Ravenfall.Models;

namespace RavenBot.Core.Ravenfall
{
    public interface ICommandProvider
    {
        ICommand GetCommand(Player redeemer, string channel, string command, string arguments);
    }
}