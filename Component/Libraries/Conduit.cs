using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ciderbit.Libraries
{
    public static class Conduit
    {
        private static TcpListener listener;
        private static List<TcpClient> clients = new List<TcpClient>();

        public static void Open()
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 3560);
            listener.Start();

            //Await clients
            Task.Run(() =>
            {
                while(true)
                {
                    if (!listener.Pending())
                        continue;

                    clients.Add(listener.AcceptTcpClient());

                    Console.WriteLine("#\tListener: client connected.");
                }
            });

            //Read stream
            Task.Run(() =>
            {
                while(true)
                {
                    foreach(var client in clients)
                    {
                        if(client.Connected)
                        {
                            var buffer = new byte[1028];
                            var stream = client.GetStream();

                            stream.Read(buffer, 0, buffer.Length);

                            Console.WriteLine($"#\tListener: data received: {Encoding.Default.GetString(buffer).Trim()}");
                        }
                    }
                }
            });
        }

        public static void Close()
        {
            listener.Stop();
        }
    }
}
