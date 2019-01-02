using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ciderbit.Common.Libraries.Conduit;

namespace Ciderbit.Component
{
	/// <summary>
	/// Component injected directly into the process. Injects assemblies into the process dynamically.
	/// </summary>
    public static class Component
    {
		private static AppDomain ScriptDomain { get; set; }

		[DllImport("kernel32")]
		static extern bool AllocConsole();

		/// <summary>
		/// Initialize the environment.
		/// </summary>
		public static void Initialize()
		{
			AllocConsole();

			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine("#Component:\tInjected.");

			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionThrown;
			AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionThrown;

			ConduitServer.DataReceived += DataReceived;

			ConduitServer.ClientConnected += (o, e) =>
			{
				Console.WriteLine("#Conduit:\tClient connected.");
				Console.WriteLine("#Conduit:\tSending data regarding currently selected process.");

				ConduitServer.Send(new ConduitPacket(ConduitPacketType.ProcessSelect, 
					Encoding.Default.GetBytes($"{Process.GetCurrentProcess().Id}")));
			};

			ConduitServer.Open();
		}

		private static void DataReceived(object sender, PacketReceivedEventArgs e)
		{
			Console.WriteLine($"#Conduit:\tReceived { e.Packet.Data?.Length } bytes.");

			switch (e.Packet.PacketType)
			{
				case ConduitPacketType.Print:
					Console.WriteLine("#Component:\n->\t" + Encoding.Default.GetString(e.Packet.Data));

					break;
				case ConduitPacketType.Execute:
					Console.WriteLine($"#Component:\tExecuting assembly.");

					var path = Encoding.Default.GetString(e.Packet.Data);

					ScriptDomain = AppDomain.CreateDomain("ScriptDomain");

					ScriptDomain.ExecuteAssembly(path);

					break;
				case ConduitPacketType.Terminate:
					Console.WriteLine($"#Component:\tAborting running assembly.");

					AppDomain.Unload(ScriptDomain);

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
