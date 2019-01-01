using System;

namespace Ciderbit.Common.Libraries.Conduit
{
	/// <summary>
	/// Arguments sent to declared DataReceived handlers.
	/// </summary>
	public class DataReceivedEventArgs : EventArgs
	{
		/// <summary>
		/// The sent packet.
		/// </summary>
		public ConduitPacket Packet;
	}
}
