using Newtonsoft.Json;
using System.Text;

namespace Ciderbit.Common.Libraries.Conduit
{
	public class ConduitPacket
	{
		[JsonProperty]
		public byte[] Data { get; set; }

		[JsonProperty]
		public ConduitPacketType PacketType { get; set; }

		[JsonConstructor]
		public ConduitPacket(ConduitPacketType packetType, byte[] data)
		{
			PacketType = packetType;
			Data = data;
		}

		public ConduitPacket(ConduitPacketType packetType) => PacketType = packetType;

		public byte[] Serialize() => Encoding.Default.GetBytes(JsonConvert.SerializeObject(this));

		public static ConduitPacket Deserialize(byte[] data) => JsonConvert.DeserializeObject<ConduitPacket>(Encoding.Default.GetString(data));
	}
}
