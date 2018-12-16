using Ciderbit.Libraries;
using Ciderbit.Types;
using System;
using System.Collections.Generic;
using System.IO;
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
        static void Main(string[] args)
        {
            var data = @"D:\GitProjects\Ciderbit\Component\bin\Debug\MyScript\first.cs;D:\GitProjects\Ciderbit\Component\bin\Debug\MyScript\second.cs";

            Conduit.Connect();

            var info = File.ReadAllText(@"D:\GitProjects\Ciderbit\Component\bin\Debug\MyScript\package.info");

            Conduit.Send(new Payload(PayloadType.Files, Encoding.Default.GetBytes(data), info));
        }
    }
}
