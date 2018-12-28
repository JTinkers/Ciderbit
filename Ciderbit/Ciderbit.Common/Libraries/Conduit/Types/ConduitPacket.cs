using Newtonsoft.Json;
using System.Text;

namespace Ciderbit.Common.Libraries.Conduit.Types
{
	/// <summary>
	/// Enum describing type of the packet.
	/// </summary>
	public enum ConduitPacketType
	{
		Print = 1,
		Execute = 2,
		Terminate = 3
	}

	/// <summary>
	/// Class describing a network packet, serialized to a JSON string and sent as bytes.
	/// </summary>
	public class ConduitPacket
	{
		/// <summary>
		/// Data stored inside the packet.
		/// </summary>
		[JsonProperty]
		public byte[] Data { get; set; }

		/// <summary>
		/// Type of the packet.
		/// </summary>
		[JsonProperty]
		public ConduitPacketType PacketType { get; set; }

		[JsonConstructor]
		public ConduitPacket(ConduitPacketType packetType, byte[] data)
		{
			PacketType = packetType;
			Data = data;
		}

		public ConduitPacket(ConduitPacketType packetType)
		{
			PacketType = packetType;
		}

		/// <summary>
		/// Serialize packet into a byte array.
		/// </summary>
		/// <returns>Serialized packet as byte array.</returns>
		public byte[] Serialize() => Encoding.Default.GetBytes(JsonConvert.SerializeObject(this));

		/// <summary>
		/// Deserialize bytes into a packet.
		/// </summary>
		/// <param name="data">Bytes to deserialize.</param>
		/// <returns>Deserialized packet.</returns>
		public static ConduitPacket Deserialize(byte[] data) => JsonConvert.DeserializeObject<ConduitPacket>(Encoding.Default.GetString(data));
	}
}
