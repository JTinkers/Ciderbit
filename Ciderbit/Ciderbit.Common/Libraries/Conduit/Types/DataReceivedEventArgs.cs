using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Common.Libraries.Conduit.Types
{
	public class DataReceivedEventArgs : EventArgs
	{
		public ConduitPacket Packet;
	}
}
