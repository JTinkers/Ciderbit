using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ciderbit.Common.Libraries.Conduit
{
	/// <summary>
	/// Class used as a container for common server/client settings.
	/// </summary>
	public static class Conduit
	{
		public static int BufferSize { get; set; } = 1024;
		public static int ConnectionRetryDelay { get; set; } = 250;
		public static int ConnectionRetryCount { get; set; } = 3;
	}
}
