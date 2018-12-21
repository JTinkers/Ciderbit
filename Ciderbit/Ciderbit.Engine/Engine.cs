using Engine.Libraries.Compiler;
using System;

namespace Ciderbit.Engine
{
	/// <summary>
	/// Class responsible for maintaining core functionality.
	/// </summary>
	public static class Engine
	{
		static void Main(string[] args)
		{
			Compiler.Create(new string[] { "test.png" }, new string[] { });
		}
	}
}
