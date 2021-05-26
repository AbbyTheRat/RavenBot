﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Shinobytes.Network
{
    public interface INetworkClient : IDisposable
    {
        Guid Id { get; }
        
        event EventHandler<DataPacket> DataReceived;
        event EventHandler Disconnected;

        void Close();
        void Send(byte[] data, int offset, int length);
    }
}
