using ChannelInterface;
using SerialChannelLib;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcpClientChannelLib;

namespace DotNetCoreSerialPortInContainer
{
    public static class ChannelBuilder
    {
        public static IChannel buildChannel(
            params string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");
            if (args.Length <=0 )
                throw new ArgumentOutOfRangeException("args must contains at least 1 element");
            switch (args[0].ToUpperInvariant())
            {
                case "TCPCLIENTCHANNEL":
                    return new TcpClientChannel(args);
                case "SERIALCHANNEL":
                    return new SerialChannel(args);
                default:
                    throw new ArgumentException($"channel type 'args[0]' unknown");
            }

        }


    }
}
