﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ROBot.Core.GameServer
{
    public class GameSessionManager : IGameSessionManager
    {
        private readonly List<IGameSession> sessions = new List<IGameSession>();
        private readonly object sessionMutex = new object();
        public event EventHandler<IGameSession> SessionStarted;
        public event EventHandler<IGameSession> SessionEnded;
        public event EventHandler<GameSessionUpdateEventArgs> SessionUpdated;
        public IGameSession Add(IBotServer server, Guid sessionId, string userId, string username, DateTime created)
        {
            lock (sessionMutex)
            {
                var existingSession = sessions.FirstOrDefault(x => x.Id == sessionId);
                if (existingSession != null)
                {
                    if (SessionStarted != null)
                        SessionStarted.Invoke(this, existingSession);

                    return existingSession;
                }

                var session = new RavenfallGameSession(server, sessionId, userId, username, created);
                sessions.Add(session);
                if (SessionStarted != null)
                    SessionStarted.Invoke(this, session);
                return session;
            }
        }

        public void Update(Guid sessionId, string twitchUserId, string newSessionName)
        {
            var session = Get(sessionId);
            var oldName = session.Name;
            session.UserId = twitchUserId;
            session.Name = newSessionName;
            SessionUpdated?.Invoke(this, new GameSessionUpdateEventArgs(session, oldName));
        }

        public void Remove(IGameSession session)
        {
            if (session == null)
                return;

            lock (sessionMutex)
            {
                if (sessions.Remove(session))
                {
                    if (SessionEnded != null)
                        SessionEnded.Invoke(this, session);
                }
            }
        }

        public void ClearAll()
        {
            lock (sessionMutex)
            {
                var s = sessions.ToList();
                foreach (var session in s)
                {
                    Remove(session);
                }
            }
        }

        public IGameSession Get(Guid id)
        {
            lock (sessionMutex)
            {
                return sessions.FirstOrDefault(x => x.Id == id);
            }
        }

        public IGameSession GetByName(string twitchUserName)
        {
            lock (sessionMutex)
            {
                return sessions.FirstOrDefault(x => x.Name.Equals(twitchUserName, StringComparison.OrdinalIgnoreCase));
            }
        }


        public IGameSession GetByUserId(string userId)
        {
            lock (sessionMutex)
            {
                return sessions.FirstOrDefault(x => x.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase));
            }
        }

        public IReadOnlyList<IGameSession> All()
        {
            lock (sessionMutex)
            {
                return sessions.ToList();
            }
        }

    }
}