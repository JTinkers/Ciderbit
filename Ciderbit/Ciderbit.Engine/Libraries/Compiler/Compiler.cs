using System;
using System.CodeDom.Compiler;
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
		private static CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");

		/// <summary>
		/// Create a compiled assembly from files specified by their paths.
		/// </summary>
		/// <param name="paths">Paths to get files from.</param>
		/// <param name="assemblies">Assemblies referenced in code.</param>
		/// <returns>Compiled assembly.</returns>
		public static Assembly Create(string[] paths, string[] references, string[] resources, string entryPoint = null)
		{
			foreach (var path in paths)
			{
				if (!File.Exists(path))
					throw new FileNotFoundException("File couldn't be used in compilation because it was not found.", path);
			}

			var parameters = new CompilerParameters();
			parameters.ReferencedAssemblies.AddRange(references); // check if files found
			parameters.EmbeddedResources.AddRange(resources); // check if files found
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