using System;
using System.CodeDom.Compiler;
using System.Text;
using Ciderbit.Libraries;

namespace Ciderbit
{
    public static class Component
    {
        public static void Initialize()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("#\tComponent: initialized.");

            Conduit.Open();

            Conduit.ClientConnected += (o, e) =>
            {
                Console.WriteLine("#\tConduit: client connected.");
            };

            Conduit.DataReceived += (o, e) =>
            {
                Console.WriteLine($"#\tConduit: script received of size {e.Data.Length} bytes");

                var script = Scripter.Compile(Encoding.Default.GetString(e.Data))?.CreateInstance("Ciderbit.Script");

                script?.GetType().GetMethod("Main").Invoke(script, null);
            };

            Scripter.CompilationFailed += (o, e) =>
            {
                Console.WriteLine("#\tScripter: compile errors occured:\n");

                foreach (CompilerError error in e.Errors)
                    Console.WriteLine($"\t[Line {error.Line}]:\t{error.ErrorText}");
            };
        }
    }
}
