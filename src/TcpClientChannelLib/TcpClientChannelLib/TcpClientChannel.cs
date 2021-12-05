using ChannelInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpClientChannelLib
{
    public class TcpClientChannel : BaseChannel, IChannel
    {
        private readonly string ip;
        private readonly int port;
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        public TcpClientChannel(string ip, string port)
        {
            this.ip = ip;
            this.port = int.Parse(port);
        }

        public TcpClientChannel(params string[] args)
        {
            int c = 1;
            string arg_ip = args[c++];
            string arg_port = args[c++];

            this.ip = arg_ip;
            if (int.TryParse(arg_port, out int port))
                this.port = port;
            else
                throw new ArgumentException("Port must be a number");
        }

        public string GetDisplayName() => $"{ip}:{port}";

        public bool Close()
        {
            if (networkStream != null)
            {
                networkStream.Flush();
                networkStream.Close();
            }

            if (tcpClient != null)
            {
                if (tcpClient.Connected)
                    tcpClient.Close();
                tcpClient.Close();
            }
            IsConnected = false;
            return true;
        }



        public bool Open()
        {
            if (tcpClient == null)
            {
                tcpClient = new TcpClient(ip, port) { NoDelay = true };
                networkStream = tcpClient.GetStream();
                IsConnected = true;
            }
            return true;
        }

        public override async Task<int> ReadAsync(byte[] buffer, CancellationToken cancellationToken)
        {
            int count = 0;

            while (!networkStream.DataAvailable && !cancellationToken.IsCancellationRequested)
                await Task.Delay(20);

            while (networkStream.DataAvailable)
            {
                int bytes = await networkStream.ReadAsync(buffer, count, buffer.Length - count, cancellationToken);
                count += bytes;
                if (bytes == 0 && count > 0) break;
                await Task.Delay(20);
            }
            return count;
        }
        public override Task WriteAsync(byte[] data, CancellationToken cancellationToken)
        => networkStream.WriteAsync(data, 0,data.Length, cancellationToken);


        private bool disposedValue;
        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (IsConnected) Close();
                    networkStream?.Dispose();
                    tcpClient?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }


    }
}
