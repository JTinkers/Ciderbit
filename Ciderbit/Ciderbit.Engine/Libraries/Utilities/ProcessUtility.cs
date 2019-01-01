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
	/// <summary>
	/// Utilitity for managing and gathering info about processes.
	/// </summary>
	public static class ProcessUtility
	{
		/// <summary>
		/// Get path from which given process is executed.
		/// </summary>
		/// <param name="process">Process to fetch execution path of.</param>
		/// <returns>Execution path.</returns>
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

		/// <summary>
		/// Get process icon.
		/// </summary>
		/// <param name="process">Process to get icon of.</param>
		/// <returns>Icon of the process.</returns>
		public static Icon GetProcessIcon(Process process)
		{
			var path = GetProcessExecutionPath(process);

			if (path == null)
				return null;

			return Icon.ExtractAssociatedIcon(path);
		}
	}
}
