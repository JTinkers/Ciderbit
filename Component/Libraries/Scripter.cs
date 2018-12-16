using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Libraries
{
    public static class Scripter
    {
        private static CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");

        public static event EventHandler<CompilationFailedEventArgs> CompilationFailed;

        public class CompilationFailedEventArgs : EventArgs
        {
            public CompilerErrorCollection Errors;
        }

        public static Assembly Compile(string code)
        {
            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            var result = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (result.Errors.HasErrors)
            {
                CompilationFailed(null, new CompilationFailedEventArgs { Errors = result.Errors });

                return null;
            }

            return result.CompiledAssembly;
        }

        public static Assembly Compile(string[] files)
        {
            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            var result = codeProvider.CompileAssemblyFromFile(parameters, files);

            if (result.Errors.HasErrors)
            {
                CompilationFailed(null, new CompilationFailedEventArgs { Errors = result.Errors });

                return null;
            }

            return result.CompiledAssembly;
        }
    }
}
