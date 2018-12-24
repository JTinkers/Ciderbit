using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Common.Libraries.Conduit.Types
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
