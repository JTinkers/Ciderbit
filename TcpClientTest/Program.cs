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
            var client = new TcpClient("127.0.0.1", 3560);

            while (!client.Connected) { }

            var stream = client.GetStream();

            var data = @"using System;
            using Ciderbit.Libraries;
            namespace Ciderbit
            {
                public static class Script
                {
                    public static void Start()
                    {
		                Scripter.Wee();
                    }
                }
            }";

            var payload = Encoding.Default.GetBytes(data.ToString()).ToList();

            //Send chunks
            for(int i = 0; i < payload.Count; i+=bufferSize)
            {
                var chunk = payload.GetRange(i, Math.Min(bufferSize, payload.Count - i)).ToArray();

                stream.Write(chunk, 0, chunk.Length);
            }

            client.Close();

            Console.Beep();
        }
    }
}
