using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
			var assembly = Compiler.Create(new string[] { AppContext.BaseDirectory + @"\Test\TestAssembly.cs" }, 
				new string[] { "System.dll" }, "TestAssembly.TestEntryPoint");

			var ye = assembly.EntryPoint.Invoke(null, BindingFlags.Public | BindingFlags.Static, null, null, CultureInfo.CurrentCulture);

			Console.WriteLine(ye);

			Console.ReadKey(true);			
		}
	}
}
