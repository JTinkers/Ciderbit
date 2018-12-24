using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ciderbit.Common.Libraries.Conduit;
using Ciderbit.Common.Libraries.Conduit.Types;
using Engine.Libraries.Compiler;

namespace Ciderbit.Engine
{
	/// <summary>
	/// Class responsible for maintaining core functionality.
	/// </summary>
	public static class Engine
	{
		static void Main(string[] args)
		{
			Compiler.Create("PrintSpammerAssembly", 
				new string[] { @"D:\GitProjects\Ciderbit\Ciderbit\Ciderbit.Engine\Data\Scripts\PrintSpammer.cs" },
				new string[] { "System.dll" }, 
				"PrintSpammerNamespace.PrintSpammer");

			if (Conduit.Connect())
			{
				var packet = new ConduitPacket(ConduitPacketType.Execute, 
					Encoding.Default.GetBytes(@"D:\GitProjects\Ciderbit\Ciderbit\Ciderbit.Engine\bin\Debug\Data\Assemblies\PrintSpammerAssembly.cider"));

				Conduit.Send(packet);

				Thread.Sleep(2000);

				packet = new ConduitPacket(ConduitPacketType.Terminate, Encoding.Default.GetBytes("PrintSpammerAssembly"));

				Conduit.Send(packet);
			}

			Conduit.Disconnect();
		}
	}
}
