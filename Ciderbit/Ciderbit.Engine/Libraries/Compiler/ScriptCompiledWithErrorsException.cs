using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;

namespace Engine.Libraries.Compiler
{
	public class ScriptCompiledWithErrorsException : Exception
	{
		public CompilerErrorCollection Errors { get; set; }

		public ScriptCompiledWithErrorsException()
		{
		}

		public ScriptCompiledWithErrorsException(string message)
			: base(message)
		{
		}

		public ScriptCompiledWithErrorsException(string message, CompilerErrorCollection errors)
			: base(message)
		{
			Errors = errors;
		}

		public ScriptCompiledWithErrorsException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public ScriptCompiledWithErrorsException(string message, Exception inner, CompilerErrorCollection errors)
			: base(message, inner)
		{
			Errors = errors;
		}
	}
}
