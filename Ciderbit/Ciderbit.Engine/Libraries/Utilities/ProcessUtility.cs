using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Engine.Libraries.Utilities
{
	public static class ProcessUtility
	{
		public static string GetProcessExecutionPath(Process process)
		{
			string path = null;

			try
			{
				//Bad exceptions :(
				path = process.MainModule.FileName;
			}
			catch { }

			return path;
		}

		public static Icon GetProcessIcon(Process process)
		{
			var path = GetProcessExecutionPath(process);

			if (path == null)
				return null;

			return Icon.ExtractAssociatedIcon(path);
		}
	}
}
