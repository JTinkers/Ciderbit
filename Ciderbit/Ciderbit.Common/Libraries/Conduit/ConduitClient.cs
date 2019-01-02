using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ciderbit.Common.Libraries.Conduit
{
	/// <summary>
	/// Class responsible for connecting to thge local TCP server, sending and receiving data.
	/// </summary>
	public static class ConduitClient
	{
		/// <summary>
		/// Called when data is received from the client.
		/// </summary>
		public static event EventHandler<PacketReceivedEventArgs> DataReceived;

		private static TcpClient client;

		/// <summary>
		/// Connect to the local TCP server.
		/// </summary>
		/// <returns>Whether the connection was succesful or not.</returns>
		public static bool Connect()
		{
			client = new TcpClient();

			for (int i = 0; i < Conduit.ConnectionRetryCount; i++)
			{
				try
				{
					client.Connect("127.0.0.1", 1964);
				}
				catch { }

				Thread.Sleep(Conduit.ConnectionRetryCount);

				if (client.Connected)
				{
					Listen();
					return true;
				}
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

			for (int i = 0; i < data.Count; i += Conduit.BufferSize)
			{
				var chunk = data.GetRange(i, Math.Min(Conduit.BufferSize, data.Count - i)).ToArray();

				stream.Write(chunk, 0, chunk.Length);
			}
		}

		private static void Listen()
		{
			Task.Run(() =>
			{
				var data = new List<byte>();
				byte[] buffer;

				while (client.Connected)
				{
					data.Clear();

					while (client.Available > 0)
					{
						buffer = new byte[Math.Min(Conduit.BufferSize, client.Available)];

						client.GetStream().Read(buffer, 0, buffer.Length);

						data.AddRange(buffer);
					}

					if (data.Count > 0)
						DataReceived(null, new PacketReceivedEventArgs { Packet = ConduitPacket.Deserialize(data.ToArray()) });
				}
			});
		}
	}
}
