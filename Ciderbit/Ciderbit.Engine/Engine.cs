using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
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
			var files = new string[] { AppContext.BaseDirectory + @"\Test\TestAssembly.cs" };
			var references = new string[] { AppContext.BaseDirectory + @"\Test\TestReferenceLib.dll", "System.dll" };
			var resources = new string[] { AppContext.BaseDirectory + @"\Test\TestReferenceLib.dll" };

			var assembly = Compiler.Create(files, references, resources, "TestAssembly.TestEntryPoint");

			//Throw on thread if freezes console
			var domain = AppDomain.CreateDomain("ScriptDOM");

			domain.ExecuteAssembly(assembly.Location);

			Console.WriteLine("HEH");
			
			Console.ReadKey(true);			
		}
	}
}
