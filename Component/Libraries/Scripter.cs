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

        public static Assembly Compile(string code)
        {
            var parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;

            var result = codeProvider.CompileAssemblyFromSource(parameters, code);

            if (result.Errors.HasErrors)
            {
                foreach (CompilerError error in result.Errors)
                    Console.WriteLine($"Column {error.Column} : Line{error.Line} - {error.ErrorText}");

                return null;
            }

            return result.CompiledAssembly;
        }
    }
}
