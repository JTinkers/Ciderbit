using System;

namespace Ciderbit.Common.Libraries.Conduit
{
	public class PacketReceivedEventArgs : EventArgs
	{
		public ConduitPacket Packet { get; set; }
	}
}
