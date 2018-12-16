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

namespace Ciderbit.Libraries
{
    /// <summary>
    /// Class responsible for TCP/IP communication between component and apps.
    /// </summary>
    public static class Conduit
    {
        private static int bufferSize = 2048;
        private static int connectionDelay = 500;

        private static TcpListener listener;
        private static List<TcpClient> clients = new List<TcpClient>();
        private static TcpClient client;

        public static event EventHandler ClientConnected;
        public static event EventHandler<DataReceivedEventArgs> DataReceived;

        public class DataReceivedEventArgs : EventArgs
        {
            public Payload Payload;
        }

        /// <summary>
        /// Connect to the locally-hosted TCP/IP connection.
        /// </summary>
        public static void Connect()
        {
            client = new TcpClient("127.0.0.1", 1964);

            Thread.Sleep(connectionDelay);
        }

        /// <summary>
        /// Send data in a payload form.
        /// </summary>
        /// <param name="payload"></param>
        public static void Send(Payload payload)
        {
            var data = Encoding.Default.GetBytes(payload.Serialize()).ToList();
            var stream = client.GetStream();

            //Send in chunks defined by bufferSize
            for (int i = 0; i < data.Count; i += bufferSize)
            {
                var chunk = data.GetRange(i, Math.Min(bufferSize, data.Count - i)).ToArray();

                stream.Write(chunk, 0, chunk.Length);
            }
        }

        /// <summary>
        /// Open a local server for communication.
        /// </summary>
        public static void Open()
        {
            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 1964);
            listener.Start();

            //Accept pending connection requests
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

            //Read stream from connected clients
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

                            while (client.Available > 0)
                            {
                                buffer = new byte[Math.Min(bufferSize, client.Available)];

                                stream.Read(buffer, 0, Math.Min(buffer.Length, client.Available));

                                data.AddRange(buffer);
                            }

                            if (data.Count > 0)
                                DataReceived(null, new DataReceivedEventArgs { Payload = Payload.Deserialize(data.ToArray()) });
                        }
                        else
                            clients.Remove(client);
                    }
                }
            });
        }

        /// <summary>
        /// Close local server and underlying connections.
        /// </summary>
        public static void Close()
        {
            clients.ForEach(x => x.Close());
            listener.Stop();
        }
    }
}
