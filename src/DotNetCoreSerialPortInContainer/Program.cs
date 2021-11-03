using System;
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
                "/dev/ttyS6" :
                "COM6";

            PrintResult(c.TryGetPrinterInfo("SerialChannel", portName, "19200", "Odd", "7", "One", "RequestToSend").Result);
            Console.WriteLine("Complete!");
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
