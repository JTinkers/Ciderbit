using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Engine.Libraries.Compiler;

namespace Ciderbit.Engine.Libraries.Compiler
{
	public static class Compiler
	{
		private static CodeDomProvider CodeProvider { get; set; } = CodeDomProvider.CreateProvider("CSharp");

		public static void Create(string output, string[] filePaths, string[] references, string entryPoint = null)
		{
			foreach (var path in filePaths)
			{
				if (!File.Exists(path))
					throw new FileNotFoundException("File couldn't be used in compilation because it was not found.", path);
			}

			var parameters = new CompilerParameters();
			parameters.OutputAssembly = output;
			parameters.ReferencedAssemblies.AddRange(references);
			parameters.GenerateExecutable = true;
			parameters.GenerateInMemory = false;
			parameters.IncludeDebugInformation = true;
			parameters.CompilerOptions = "/optimize";
			parameters.MainClass = entryPoint;

			var results = CodeProvider.CompileAssemblyFromFile(parameters, filePaths);

			if (results.Errors.HasErrors)
				throw new ScriptCompiledWithErrorsException("Assembly compiled with errors.", results.Errors);
		}
	}
}