using ChannelInterface;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SerialChannelLib
{
    public class SerialChannel : BaseChannel, IChannel
    {
        private SerialPort serialPort;
        private readonly string portName;
        private readonly int baudRate;
        private readonly Parity parity;
        private readonly int dataBits;
        private readonly StopBits stopBits;
        private readonly Handshake handshake;

        public SerialChannel(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake)
        {
            this.portName = portName;
            this.baudRate = baudRate;
            this.parity = parity;
            this.dataBits = dataBits;
            this.stopBits = stopBits;
            this.handshake = handshake;
        }

        public SerialChannel(params string[] args)
        {
            int c = 1;
            string arg_portName = args[c++];
            string arg_baudRate = args[c++];
            string arg_parity = args[c++];
            string arg_dataBits = args[c++];
            string arg_stopBits = args[c++];
            string arg_handshake = args[c++];

            portName = arg_portName;

            if (int.TryParse(arg_baudRate, out int baudRate))
                this.baudRate = baudRate;
            else
                throw new ArgumentException("BaudRate must be a number");

            if (Enum.TryParse(arg_parity, out Parity parity))
                this.parity = parity;
            else
                throw new ArgumentException("Parity arg unknown");


            if (int.TryParse(arg_dataBits, out int dataBits))
                this.dataBits = dataBits;
            else
                throw new ArgumentException("DataBits must be a number");

            if (Enum.TryParse(arg_stopBits, out StopBits stopBits))
                this.stopBits = stopBits;
            else
                throw new ArgumentException("stopBits arg unknown");


            if (Enum.TryParse(arg_handshake, out Handshake handshake))
                this.handshake = handshake;
            else
                throw new ArgumentException("Handshake arg unknown");

        }

        public string GetDisplayName()
        => $"{portName}:{baudRate}:{parity}:{dataBits}:{stopBits}:{handshake}";

        public bool Open()
        {
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.Handshake = handshake;
            serialPort.Open();
            return true;
        }
        public bool Close()
        {
            if (serialPort.IsOpen)
                serialPort?.BaseStream.Flush();
            serialPort?.Close();
            return true;
        }

        public override Task WriteAsync(byte[] data, CancellationToken cancellationToken)
        => serialPort.BaseStream.WriteAsync(data, 0, data.Length, cancellationToken);


        public override async Task<int> ReadAsync(byte[] buffer, CancellationToken cancellationToken)
        {
            int count = 0;

            while (serialPort.BytesToRead == 0 && !cancellationToken.IsCancellationRequested)
                await Task.Delay(20);

            while (serialPort.BytesToRead > 0)
            {
                int bytes = await serialPort.BaseStream.ReadAsync(buffer, count, buffer.Length - count, cancellationToken);
                count += bytes;
                if (bytes == 0 && count > 0) break;
                await Task.Delay(20);
            }
            return count;
        }

        public override bool IsConnected
        {
            get => serialPort?.IsOpen ?? false;
        }

        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (serialPort != null)
                    {
                        if (serialPort.IsOpen)
                            serialPort.Close();
                        serialPort.Dispose();
                    }
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
