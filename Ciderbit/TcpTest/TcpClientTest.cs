using Ciderbit.Common.Libraries.Conduit;
using Ciderbit.Common.Libraries.Conduit.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TcpTest
{
	class TcpClientTest
	{
		static void Main(string[] args)
		{
			if (Conduit.Connect())
			{
				var packet = new ConduitPacket(ConduitPacketType.Print, Encoding.Default.GetBytes(Console.ReadLine()));

				Conduit.Send(packet);
			}

			Conduit.Disconnect();
		}
	}
}
