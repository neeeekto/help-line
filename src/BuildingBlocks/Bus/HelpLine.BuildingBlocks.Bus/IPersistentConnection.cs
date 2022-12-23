using System;

namespace HelpLine.BuildingBlocks.Bus
{
    public interface IPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();
    }
}
