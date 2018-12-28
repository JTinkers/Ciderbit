using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Ciderbit.Common.Libraries.Conduit;
using Ciderbit.Common.Libraries.Conduit.Types;
using Ciderbit.Engine.Libraries.UI;
using Ciderbit.Engine.Libraries.AssemblyCompiler;

namespace Ciderbit.Engine
{
	/// <summary>
	/// Class responsible for maintaining core functionality.
	/// </summary>
	public static class Engine
	{
		[STAThread]
		static void Main(string[] args)
		{
			var frame = new MainFrame();
			frame.ShowDialog();
		}
	}
}
