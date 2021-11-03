using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelInterface
{
    public interface IChannel: IDisposable
    {
        bool IsConnected {get;}
        string GetDisplayName();
        bool Open();
        bool Close();
        Task WriteAsync(byte[] data);
        Task WriteAsync(byte[] data, CancellationToken cancellationToken);
        Task<int> ReadAsync(byte[] buffer);
        Task<int> ReadAsync(byte[] buffer, CancellationToken cancellationToken);

    }
}