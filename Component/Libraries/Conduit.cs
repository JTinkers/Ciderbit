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
        private static int bufferSize = 512;

        private static TcpListener listener;
        private static List<TcpClient> clients = new List<TcpClient>();

        public static event EventHandler ClientConnected;
        public static event EventHandler<DataReceivedEventArgs> DataReceived;

        public class DataReceivedEventArgs : EventArgs
        {
            public byte[] Data;
        }

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

                    ClientConnected(null, EventArgs.Empty);
                }
            });

            //Read stream
            Task.Run(() =>
            {
                while(true)
                {
                    var data = new List<byte>();
                    var buffer = new byte[bufferSize];

                    foreach(var client in clients)
                    {
                        data.Clear();
                        if (client.Connected)
                        {
                            var stream = client.GetStream();

                            while(client.Available > 0)
                            {
                                buffer = new byte[Math.Min(bufferSize, client.Available)];

                                stream.Read(buffer, 0, Math.Min(buffer.Length, client.Available));

                                data.AddRange(buffer);
                            }

                            if (data.Count > 0)
                                DataReceived(null, new DataReceivedEventArgs { Data = data.ToArray() });
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
