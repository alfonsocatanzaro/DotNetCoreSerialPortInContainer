using System;
using System.Diagnostics;
using System.IO.Ports;

namespace DotNetCoreSerialPortInContainer
{
    class Program
    {
        static void Main(string[] args)
        {
            var c = new CustomPrinterDetective();
            //PrintResult(c.TryGetPrinterInfo("192.168.64.145", 9100).Result);
            //PrintResult(c.TryGetPrinterInfo("192.168.64.145", 9100).Result);
            //PrintResult(c.TryGetPrinterInfo("192.168.64.145", 9100).Result);
            //PrintResult(c.TryGetPrinterInfo("192.168.64.145", 9100).Result);
            //PrintResult(c.TryGetPrinterInfo("192.168.64.145", 9100).Result);
            //PrintResult(c.TryGetPrinterInfo("192.168.64.145", 9100).Result);
            //PrintResult(c.TryGetPrinterInfo("192.168.64.145", 9100).Result);
            //PrintResult(c.TryGetPrinterInfo("192.168.64.145", 9100).Result);
            string portName = Environment.OSVersion.Platform == PlatformID.Unix ?
                "/dev/ttyS3" :
                "COM7";

            string localHostName = Environment.OSVersion.Platform == PlatformID.Unix ?
                "host.docker.internal" :
                "localhost";
            // localHostName = "host.docker.internal";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            PrintResult(c.TryGetPrinterInfo("SerialChannel", portName, "19200", "Odd", "7", "One", "RequestToSend").Result);
            
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);
            //PrintResult(c.TryGetPrinterInfo("TcpClientChannel", localHostName, "22101").Result);

            Console.WriteLine("Complete!");
            stopwatch.Stop();
            Console.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds}ms");
            Console.ReadLine();
        }

        private static void PrintResult(CustomPrinterInfo result)
        {
            Console.Write($"Channel:{result.ChannelName}");
            if (result.Success)
                Console.WriteLine($" - serial:{result.Serial}");
            else
                Console.WriteLine($" - ERROR: {result.FailReason}");
        }
    }
}
