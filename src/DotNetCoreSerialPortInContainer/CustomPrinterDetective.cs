using ChannelInterface;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreSerialPortInContainer
{
    public class CustomPrinterDetective
    {

        private byte[] command = new byte[] {
            // Start frame
            0x02,
            // frame count "00"
            0x30, 0x30,
            // Identifier "0"
            0x30,
            // Command "1511"
            0x31, 0x35, 0x31, 0x31,
            // Checksum "44"
            0x34, 0x34,
            // End frame
            0x03
        };

        public async Task<CustomPrinterInfo> TryGetPrinterInfo(params string [] args)
        {

            CustomPrinterInfo result;
            using (var channel = ChannelBuilder.buildChannel(args))
            {
                result = await TryGetPrinterInfo(channel);
            }
            return result;

        }

        public async Task<CustomPrinterInfo> TryGetPrinterInfo(IChannel channel)
        {
            try
            {
                channel.Open();

                var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(10));

                cancellationTokenSource.Token.Register(() => channel.Close());

                await channel.WriteAsync(command, cancellationTokenSource.Token);

                var receiveBuffer = new byte[256];

                int bytes = await channel.ReadAsync(receiveBuffer, cancellationTokenSource.Token);

                channel.Close();

                if (bytes == 0)
                    throw new CustomPrinterDetectiveException(channel, "0 bytes received");
                if (bytes < 50)
                    throw new CustomPrinterDetectiveException(channel, "Response too short");
                if (receiveBuffer[0] == 0x15)
                    throw new CustomPrinterDetectiveException(channel, "NACK received");
                if (receiveBuffer[0] != 0x06)
                    throw new CustomPrinterDetectiveException(channel, "Not ACK received");
                if (Encoding.UTF8.GetString(receiveBuffer.Skip(5).Take(4).ToArray()) != "1511")
                    throw new CustomPrinterDetectiveException(channel, "Mismatch command echo ");


                return CustomPrinterInfo.Ok(
                    channel,
                    Encoding.UTF8.GetString(
                        receiveBuffer
                        .Skip(10)
                        .Take(11)
                        .ToArray()
                ));


            }
            catch (CustomPrinterDetectiveException ex)
            {
                Console.WriteLine(ex.Message);
                return CustomPrinterInfo.Fails(channel, ex.Message);
            }
            catch (TaskCanceledException)
            {
                return CustomPrinterInfo.Fails(channel, "Communication timeout");
            }
            catch (Exception ex)
            {
                return CustomPrinterInfo.Fails(channel, ex);
            }

        }



    }


    public class CustomPrinterDetectiveException : Exception
    {
        public string ChannelName { get; set; }
        public CustomPrinterDetectiveException(IChannel channel, string reason) : base($"'{channel.GetDisplayName()}' isn't a Custom printer: {reason}")
        {
            ChannelName = channel.GetDisplayName();
        }
    }

    public class CustomPrinterInfo
    {
        private CustomPrinterInfo() { }
        public static CustomPrinterInfo Fails(IChannel channel, string reason)
        {
            return new CustomPrinterInfo()
            {
                ChannelName = channel.GetDisplayName(),
                FailReason = reason,
                Success = false
            };
        }

        public static CustomPrinterInfo Fails(IChannel channel, Exception exception)
        {
            return new CustomPrinterInfo()
            {
                ChannelName = channel.GetDisplayName(),
                FailReason = exception.Message,
                Exception = exception,
                Success = false
            };
        }

        public static CustomPrinterInfo Ok(IChannel channel, string serial)
        {
            return new CustomPrinterInfo()
            {
                ChannelName = channel.GetDisplayName(),
                Serial = serial,
                Success = true
            };
        }


        public bool Success { get; private set; }
        public string ChannelName { get; private set; }
        public string Serial { get; private set; } = "";
        public string FailReason { get; private set; } = "";
        public Exception Exception { get; private set; } = null;

    }



}
