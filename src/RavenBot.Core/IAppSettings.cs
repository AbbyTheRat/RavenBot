﻿namespace RavenBot.Core
{
    public interface IAppSettings
    {
        string TwitchBotUsername { get; }
        string TwitchBotAuthToken { get; }
        string TwitchChannel { get; }
    }
}