using System;
using System.Text;
using System.IO;
using System.Linq;
using Ciderbit.Engine.UI;
using Ciderbit.Engine.Libraries.Compiler;
using Ciderbit.Common.Libraries.Conduit;

namespace Ciderbit.Engine
{
	public static class Engine
	{
		private static string ScriptDirectory { get; set; } = AppContext.BaseDirectory + @"Data\Scripts";

		public static string[] GetScriptDirectories() => Directory.GetDirectories(ScriptDirectory);

		public static Script[] GetScripts() => GetScriptDirectories().Select(path => new Script(path)).ToArray();

		public static void TerminateScript()
		{
			Console.WriteLine("Sending termination packet.");

			ConduitClient.Send(new ConduitPacket(ConduitPacketType.Terminate));
		}

		public static void ExecuteScript(Script script)
		{
			Console.WriteLine($"Compiling: {script.Info.Name}.");

			script.Compile();

			Console.WriteLine($"Sending execution packet.");

			ConduitClient.Send(new ConduitPacket(ConduitPacketType.Execute, Encoding.Default.GetBytes(script.OutputPath)));
		}

		[STAThread]
		static void Main(string[] args)
		{
			var frame = new MainFrame();
			frame.ShowDialog();
		}
	}
}
