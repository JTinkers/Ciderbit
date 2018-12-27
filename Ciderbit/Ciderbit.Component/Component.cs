using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ciderbit.Common.Libraries.Conduit;
using Ciderbit.Common.Libraries.Conduit.Types;

namespace Ciderbit.Component
{
	/// <summary>
	/// Component injected directly into the process. Injects assemblies into the process dynamically.
	/// </summary>
    public static class Component
    {
		private static Dictionary<string, AppDomain> runningAssemblies { get; set; } = new Dictionary<string, AppDomain>();

		/// <summary>
		/// Initialize the environment.
		/// </summary>
		public static void Initialize()
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("#Component:\tInjected.");

			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionThrown;
			AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionThrown;

			Conduit.DataReceived += DataReceived;
			Conduit.ClientConnected += (o, e) =>
			{
				Console.WriteLine("#Conduit:\tClient connected.");
			};

			Conduit.Open();
		}

		private static void DataReceived(object sender, DataReceivedEventArgs e)
		{
			Console.WriteLine($"#Conduit:\tReceived { e.Packet.Data.Length } bytes.");

			string path, name;
			switch (e.Packet.PacketType)
			{
				case ConduitPacketType.Print:
					Console.WriteLine("#Component:");
					Console.WriteLine("->\t" + Encoding.Default.GetString(e.Packet.Data));
					break;
				case ConduitPacketType.Execute:
					path = Encoding.Default.GetString(e.Packet.Data);
					name = Path.GetFileNameWithoutExtension(path);

					Console.WriteLine($"#Component:\tExecuting assembly [{name}]");

					var domain = AppDomain.CreateDomain(name);

					Task.Run(() => domain.ExecuteAssembly(path));

					runningAssemblies[name] = domain;
					break;
				case ConduitPacketType.Terminate:
					name = Path.GetFileNameWithoutExtension(Encoding.Default.GetString(e.Packet.Data));

					Console.WriteLine($"#Component:\tAborting running assembly [{name}]");

					//if(runningAssemblies.ContainsKey(name))
						AppDomain.Unload(runningAssemblies[name]);

					break;
			}
		}

		private static void FirstChanceExceptionThrown(object sender, FirstChanceExceptionEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("#Component:\tException:\t" + e.Exception.Message);
			Console.WriteLine("\t\tTargetSite:\t" + e.Exception.TargetSite);
			Console.WriteLine("\t\tInnerException:\t" + e.Exception.InnerException);
			Console.WriteLine("\t\tSource:\t" + e.Exception.Source);
			Console.WriteLine("\t\tStackTrace:\t" + e.Exception.StackTrace);
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.BackgroundColor = ConsoleColor.Black;
		}

		private static void UnhandledExceptionThrown(object sender, UnhandledExceptionEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Black;
			Console.BackgroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("#Component:\tUnhandled exception.");
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.BackgroundColor = ConsoleColor.Black;
		}
	}
}
