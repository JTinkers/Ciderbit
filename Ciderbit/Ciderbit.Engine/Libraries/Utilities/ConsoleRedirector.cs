using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Engine.Libraries.Utilities
{
	public class ConsoleRedirector : TextWriter
	{
		public event EventHandler<TextWrittenEventArgs> TextWritten;

		public override void WriteLine(string value)
		{
			base.WriteLine();

			TextWritten(null, new TextWrittenEventArgs() { Text = value });
		}

		public override Encoding Encoding => Encoding.UTF8;
	}
}
