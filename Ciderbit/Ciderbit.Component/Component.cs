using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Ciderbit.Common.Libraries.Conduit;

namespace Ciderbit.Component
{
    public static class Component
    {
		public enum OutputColor
		{
			Default = ConsoleColor.Cyan,
			Error = ConsoleColor.Red
		}

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
				Console.WriteLine($"#Conduit:\tReceived { e.Packet.Data.Length }B.");
			};

			Conduit.Open();
		}

		private static void FirstChanceExceptionThrown(object sender, FirstChanceExceptionEventArgs e)
		{
			Console.ForegroundColor = (ConsoleColor)OutputColor.Error;
			Console.WriteLine("#Component:\tException:" + e.Exception.Message);
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
