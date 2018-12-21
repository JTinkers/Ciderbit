using System;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom.Compiler;
using System.Text;
using System.IO;
using Engine.Libraries.Compiler.Types;

namespace Engine.Libraries.Compiler
{

	/// <summary>
	/// Class with set of functions for compiling C# files and code into assemblies.
	/// </summary>
	public static class Compiler
	{
		private static CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");

		/// <summary>
		/// Create a compiled assembly from files specified by their paths.
		/// </summary>
		/// <param name="paths">Paths to get files from.</param>
		/// <param name="assemblies">Assemblies referenced in code.</param>
		/// <returns>Compiled assembly.</returns>
		public static Assembly Create(string[] paths, string[] assemblies)
		{
			foreach (var path in paths)
			{
				if (!File.Exists(path))
					throw new FileNotFoundException("File couldn't be used in compilation because it was not found.", path);
			}

			var parameters = new CompilerParameters();
			parameters.ReferencedAssemblies.AddRange(assemblies);
			parameters.GenerateExecutable = false;
			parameters.GenerateInMemory = true;

			var results = codeProvider.CompileAssemblyFromFile(parameters, paths);

			if (results.Errors.HasErrors)
				throw new AssemblyCompiledWithErrorsException("Assembly compiled with errors", results.Errors);

			return results.CompiledAssembly;
		}
	}
}
