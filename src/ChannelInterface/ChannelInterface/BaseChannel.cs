using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChannelInterface
{
    public abstract class BaseChannel
    {
        private bool isConnected = false;
        public virtual bool IsConnected { get => isConnected; protected set => isConnected=value;  }


        public Task WriteAsync(byte[] data) => WriteAsync(data, CancellationToken.None);
        public abstract Task WriteAsync(byte[] data, CancellationToken cancellationToken);


        public Task<int> ReadAsync(byte[] buffer) => ReadAsync(buffer, CancellationToken.None);
        public abstract Task<int> ReadAsync(byte[] buffer, CancellationToken cancellationToken);

    }
}