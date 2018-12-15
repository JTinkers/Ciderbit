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
        static void Main(string[] args)
        {
            var client = new TcpClient("127.0.0.1", 3560);

            var stream = client.GetStream();
            
            var data = "So save me, before I fall.";

            stream.Write(Encoding.Default.GetBytes(data), 0, data.Length);

            Console.WriteLine($"Sending: {data}");

            Console.ReadKey(true);
        }
    }
}
