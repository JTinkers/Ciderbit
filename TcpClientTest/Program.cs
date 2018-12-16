using Ciderbit.Libraries;
using Ciderbit.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TcpClientTest
{
    class Program
    {
        private static int bufferSize = 512;

        static void Main(string[] args)
        {
            var data = @"D:\GitProjects\Ciderbit\Component\bin\Debug\first.txt;D:\GitProjects\Ciderbit\Component\bin\Debug\second.txt";

            Conduit.Connect();

            Thread.Sleep(500);

            Conduit.Send(new Payload(PayloadType.Files, Encoding.Default.GetBytes(data)));
        }
    }
}
