using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Common.Libraries.Conduit
{
	/// <summary>
	/// Class responsible for establishing a local TCP server.
	/// </summary>
	public static class ConduitServer
	{
		/// <summary>
		/// Called when client connects to the server succesfully.
		/// </summary>
		public static event EventHandler ClientConnected;

		/// <summary>
		/// Called when data is received from the client.
		/// </summary>
		public static event EventHandler<PacketReceivedEventArgs> DataReceived;

		private static TcpListener server;
		private static List<TcpClient> clients;

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

		public static void Send(ConduitPacket packet)
		{
			//Remove disconnected clients
			clients = clients.Where(x => x.Connected).ToList();

			var data = packet.Serialize().ToList();

			foreach (var client in clients)
			{
				var stream = client.GetStream();

				for (int i = 0; i < data.Count; i += Conduit.BufferSize)
				{
					var chunk = data.GetRange(i, Math.Min(Conduit.BufferSize, data.Count - i)).ToArray();

					stream.Write(chunk, 0, chunk.Length);
				}
			}
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
								buffer = new byte[Math.Min(Conduit.BufferSize, client.Available)];

								stream.Read(buffer, 0, buffer.Length);

								data.AddRange(buffer);
							}

							if (data.Count > 0)
								DataReceived(null, new PacketReceivedEventArgs { Packet = ConduitPacket.Deserialize(data.ToArray()) });
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
