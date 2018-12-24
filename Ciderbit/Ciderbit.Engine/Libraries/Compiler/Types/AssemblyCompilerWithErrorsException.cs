using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace Engine.Libraries.Compiler.Types
{
	/// <summary>
	/// Exception type thrown when assembly compiled with errors.
	/// </summary>
	public class AssemblyCompiledWithErrorsException : Exception
	{
		public CompilerErrorCollection Errors { get; set; }

		public AssemblyCompiledWithErrorsException()
		{
		}

		public AssemblyCompiledWithErrorsException(string message)
			: base(message)
		{
		}

		public AssemblyCompiledWithErrorsException(string message, CompilerErrorCollection errors)
			: base(message)
		{
			Errors = errors;
		}

		public AssemblyCompiledWithErrorsException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public AssemblyCompiledWithErrorsException(string message, Exception inner, CompilerErrorCollection errors)
			: base(message, inner)
		{
			Errors = errors;
		}
	}
}
