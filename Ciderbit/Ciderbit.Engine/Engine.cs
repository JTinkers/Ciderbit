using System;
using System.Diagnostics;
using Ciderbit.Engine.UI;

namespace Ciderbit.Engine
{
	/// <summary>
	/// Class responsible for core functionality.
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
