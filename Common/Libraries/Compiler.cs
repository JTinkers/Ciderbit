using Ciderbit.Types;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ciderbit.Libraries
{
    /// <summary>
    /// Class responsible for creating compiled assemblies
    /// </summary>
    public static class Compiler
    {
        private static CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");

        public static event EventHandler<CompilationFailedEventArgs> CompilationFailed;

        public class CompilationFailedEventArgs : EventArgs
        {
            public CompilerErrorCollection Errors;
        }

        /// <summary>
        /// Create a compiled assembly from string of code.
        /// </summary>
        /// <param name="code">Code to compile into an assembly.</param>
        /// <returns>Compiled assembly.</returns>
        public static Assembly Create(string code)
        {
            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("CiderbitCommon.dll");
            parameters.ReferencedAssemblies.Add("CiderbitComponent.dll");
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

        /// <summary>
        /// Create a compiled assembly from files.
        /// </summary>
        /// <param name="files">Files to compile into assembly.</param>
        /// <param name="assemblies">Referenced assemblies to include in the compilation.</param>
        /// <returns>Compiled assembly.</returns>
        public static Assembly Create(string[] files, string[] assemblies)
        {
            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.AddRange(assemblies);
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
