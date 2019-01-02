using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ciderbit.Common.Libraries.Conduit
{
	public static class ConduitClient
	{
		public static event EventHandler<PacketReceivedEventArgs> DataReceived;

		private static TcpClient Client { get; set; }

		public static bool Connect()
		{
			Client = new TcpClient();

			for (int i = 0; i < Conduit.ConnectionRetryCount; i++)
			{
				try
				{
					Client.Connect("127.0.0.1", 1964);
				}
				catch { }

				Thread.Sleep(Conduit.ConnectionRetryCount);

				if (Client.Connected)
				{
					Listen();
					return true;
				}
			}

			return false;
		}

		public static void Disconnect() => Client.Close();

		public static void Send(ConduitPacket packet)
		{
			var data = packet.Serialize().ToList();
			var stream = Client.GetStream();

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

				while (Client.Connected)
				{
					data.Clear();

					while (Client.Available > 0)
					{
						buffer = new byte[Math.Min(Conduit.BufferSize, Client.Available)];

						Client.GetStream().Read(buffer, 0, buffer.Length);

						data.AddRange(buffer);
					}

					if (data.Count > 0)
						DataReceived(null, new PacketReceivedEventArgs { Packet = ConduitPacket.Deserialize(data.ToArray()) });
				}
			});
		}
	}
}
