using Ciderbit.Common.Libraries.Conduit.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ciderbit.Common.Libraries.Conduit
{
	/// <summary>
	/// Class used in establishing a local server-client connection.
	/// </summary>
	public static class Conduit
	{
		/// <summary>
		/// Called when client connects to the server succesfully.
		/// </summary>
		public static event EventHandler ClientConnected;

		/// <summary>
		/// Called when data is received from the client.
		/// </summary>
		public static event EventHandler<DataReceivedEventArgs> DataReceived;

		private static int bufferSize { get; set; } = 1024;
		private static int connectionRetryDelay { get; set; } = 250;
		private static int connectionRetryCount { get; set; } = 3;

		private static TcpListener server { get; set; }
		private static TcpClient client { get; set; }
		private static List<TcpClient> clients { get; set; }

		/// <summary>
		/// Connect to the local TCP server.
		/// </summary>
		/// <returns>Whether the connection was succesful or not.</returns>
		public static bool Connect()
		{
			client = new TcpClient();

			for (int i = 0; i < connectionRetryCount; i++)
			{
				try
				{
					client.Connect("127.0.0.1", 1964);
				}
				catch { }

				Thread.Sleep(connectionRetryDelay);

				if (client.Connected)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Disconnect from the local server.
		/// </summary>
		public static void Disconnect() => client.Close();

		/// <summary>
		/// Send a data packet through the local network.
		/// </summary>
		/// <param name="packet">Packet along with the type and data to pass.</param>
		public static void Send(ConduitPacket packet)
		{
			var data = packet.Serialize().ToList();
			var stream = client.GetStream();

			for (int i = 0; i < data.Count; i += bufferSize)
			{
				var chunk = data.GetRange(i, Math.Min(bufferSize, data.Count - i)).ToArray();

				stream.Write(chunk, 0, chunk.Length);
			}
		}

		/// <summary>
		/// Open local server for connection.
		/// </summary>
		public static void Open()
		{
			server = new TcpListener(IPAddress.Parse("127.0.0.1"), 1964);
			server.Start();

			clients = new List<TcpClient>();

			Listen();
		}

		/// <summary>
		/// Close local server and underlying connections.
		/// </summary>
		public static void Close()
		{
			clients.ForEach(x => x.Close());
			server.Stop();
		}

		private static void Listen()
		{
			Task.Run(() =>
			{
				var data = new List<byte>();
				byte[] buffer;

				while (true)
				{

					foreach (var client in clients)
					{
						data.Clear();
						if (client.Connected)
						{
							var stream = client.GetStream();

							while (client.Available > 0)
							{
								buffer = new byte[Math.Min(bufferSize, client.Available)];

								stream.Read(buffer, 0, buffer.Length);

								data.AddRange(buffer);
							}

							if (data.Count > 0)
								DataReceived(null, new DataReceivedEventArgs { Packet = ConduitPacket.Deserialize(data.ToArray()) });
						}
					}

					if (!server.Pending())
						continue;

					clients.Add(server.AcceptTcpClient());

					ClientConnected(null, EventArgs.Empty);
				}
			});
		}
	}
}
