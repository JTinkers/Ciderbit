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
using Engine.Libraries.Compiler;

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

			/*var scriptPath = @"D:\GitProjects\Ciderbit\Ciderbit\Ciderbit.Engine\Data\Scripts\PrintSpammer\";

			Compiler.Create(scriptPath + "PrintSpammer.cider", 
				Directory.GetFiles(scriptPath, "*.cs", SearchOption.AllDirectories),
				Directory.GetFiles(scriptPath, "*.dll", SearchOption.AllDirectories), 
				"PrintSpammerNamespace.PrintSpammer");

			if (Conduit.Connect())
			{
				var packet = new ConduitPacket(ConduitPacketType.Execute, 
					Encoding.Default.GetBytes(scriptPath + "PrintSpammer.cider"));

				Conduit.Send(packet);

				Thread.Sleep(2000);

				packet = new ConduitPacket(ConduitPacketType.Terminate, Encoding.Default.GetBytes("PrintSpammerAssembly"));

				Conduit.Send(packet);
			}

			Conduit.Disconnect();*/
		}
	}
}
