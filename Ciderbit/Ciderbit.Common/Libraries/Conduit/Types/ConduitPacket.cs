using Newtonsoft.Json;
using System.Text;

namespace Ciderbit.Common.Libraries.Conduit.Types
{
	public enum ConduitPacketType
	{
		Print = 1,
		Execute = 2,
		Suspend = 3,
		Terminate = 4
	}

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

		public byte[] Serialize() => Encoding.Default.GetBytes(JsonConvert.SerializeObject(this));

		public static ConduitPacket Deserialize(byte[] data) => JsonConvert.DeserializeObject<ConduitPacket>(Encoding.Default.GetString(data));
	}
}
