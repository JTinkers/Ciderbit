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
    public static class Component
    {
		private enum OutputColor
		{
			Default = ConsoleColor.Cyan,
			Error = ConsoleColor.Red,
			Print = ConsoleColor.DarkGreen
		}

		private static Dictionary<string, AppDomain> runningAssemblies { get; set; } = new Dictionary<string, AppDomain>();

		public static void Initialize()
		{
			Console.ForegroundColor = (ConsoleColor)OutputColor.Default;
			Console.WriteLine("#Component:\tInjected.");

			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionThrown;
			AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionThrown;

			Conduit.ClientConnected += (o, e) =>
			{
				Console.WriteLine("#Conduit:\tClient connected.");
			};

			Conduit.DataReceived += (o, e) =>
			{
				Console.WriteLine($"#Conduit:\tReceived { e.Packet.Data.Length } bytes.");

				string path, name;
				switch(e.Packet.PacketType)
				{
					case ConduitPacketType.Print:
						Console.WriteLine("#Component:");
						Console.ForegroundColor = (ConsoleColor)OutputColor.Print;
						Console.WriteLine("->\t" + Encoding.Default.GetString(e.Packet.Data));
						Console.ForegroundColor = (ConsoleColor)OutputColor.Default;
						break;
					case ConduitPacketType.Execute:
						path = Encoding.Default.GetString(e.Packet.Data);
						name = Path.GetFileNameWithoutExtension(path);

						Console.WriteLine($"#Component:\tExecuting assembly [{name}]");

						var domain = AppDomain.CreateDomain(name);
						domain.ExecuteAssembly(path);

						runningAssemblies.Add(name, domain);
						break;
					case ConduitPacketType.Terminate:
						name = Path.GetFileNameWithoutExtension(Encoding.Default.GetString(e.Packet.Data));

						Console.WriteLine($"#Component:\tAborting running assembly [{name}]");

						AppDomain.Unload(runningAssemblies[name]);
						break;
				}
			};

			Conduit.Open();
		}

		private static void FirstChanceExceptionThrown(object sender, FirstChanceExceptionEventArgs e)
		{
			Console.ForegroundColor = (ConsoleColor)OutputColor.Error;
			Console.WriteLine("#Component:\tException: " + e.Exception.Message);
			Console.ForegroundColor = (ConsoleColor)OutputColor.Default;
		}

		private static void UnhandledExceptionThrown(object sender, UnhandledExceptionEventArgs e)
		{
			Console.ForegroundColor = (ConsoleColor)OutputColor.Error;
			Console.WriteLine("#Component:\tUnhandled exception.");
			Console.ForegroundColor = (ConsoleColor)OutputColor.Default;
		}
	}
}
