using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Common.Libraries.Conduit
{
	public enum ConduitPacketType
	{
		Print,
		Execute,
		Terminate,
		ProcessSelect,
		ScriptFinished
	}
}
