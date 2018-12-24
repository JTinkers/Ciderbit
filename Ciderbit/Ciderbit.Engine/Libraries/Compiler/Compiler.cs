﻿using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Engine.Libraries.Compiler.Types;

namespace Engine.Libraries.Compiler
{
	/// <summary>
	/// Class with set of functions for compiling C# files and code into assemblies.
	/// </summary>
	public static class Compiler
	{
		private static CodeDomProvider codeProvider { get; set; } = CodeDomProvider.CreateProvider("CSharp");

		/// <summary>
		/// Create a compiled assembly from files specified by their paths.
		/// </summary>
		/// <param name="name">Name of the assembly.</param>
		/// <param name="paths">Path of files to compile into an assembly.</param>
		/// <param name="references">Referenced libraries used by the assembly.</param>
		/// <param name="entryPoint">Class containing static Main entry point.</param>
		/// <returns></returns>
		public static Assembly Create(string name, string[] paths, string[] references, string entryPoint = null)
		{
			foreach (var path in paths)
			{
				if (!File.Exists(path))
					throw new FileNotFoundException("File couldn't be used in compilation because it was not found.", path);
			}

			var parameters = new CompilerParameters();
			parameters.OutputAssembly = AppContext.BaseDirectory + @"/Data/Scripts/" + name + ".cider";
			parameters.ReferencedAssemblies.AddRange(references);
			parameters.GenerateExecutable = true;
			parameters.GenerateInMemory = false;
			parameters.IncludeDebugInformation = true;
			parameters.CompilerOptions = "/optimize";
			parameters.MainClass = entryPoint;

			var results = codeProvider.CompileAssemblyFromFile(parameters, paths);

			if (results.Errors.HasErrors)
				throw new AssemblyCompiledWithErrorsException("Assembly compiled with errors.", results.Errors);

			return results.CompiledAssembly;
		}
	}
}