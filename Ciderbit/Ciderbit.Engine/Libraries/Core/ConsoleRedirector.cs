using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Engine.Libraries.Core
{
	/// <summary>
	/// Class used in redirection of console output.
	/// </summary>
	public class ConsoleRedirector : TextWriter
	{
		public event EventHandler<LineWrittenEventArgs> LineWritten;

		public override void WriteLine(string value)
		{
			base.WriteLine();

			LineWritten(null, new LineWrittenEventArgs() { Line = value });
		}

		public override Encoding Encoding => Encoding.UTF8;
	}
}
