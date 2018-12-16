using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Ciderbit.Libraries;

namespace Ciderbit
{
    public static class Component
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AllocConsole();

        public static void Initialize()
        {
            AllocConsole();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("#\tComponent: initialized.");

            Scripter.CompilationFailed += (o, e) =>
            {
                Console.WriteLine("#\tScripter: compile errors occured:\n");

                foreach (CompilerError error in e.Errors)
                    Console.WriteLine($"\t[Line {error.Line}]:\t{error.ErrorText}");
            };

            Conduit.Open();

            Conduit.ClientConnected += (o, e) =>
            {
                Console.WriteLine("#\tConduit: client connected.");
            };

            Conduit.DataReceived += (o, e) =>
            {
                Console.WriteLine($"#\tConduit: script received of size {e.Data.Length} bytes");

                var script = Scripter.Compile(Encoding.Default.GetString(e.Data)).GetType("Ciderbit.Script", true, false);

                var method = script.GetMethod("Start", BindingFlags.Public | BindingFlags.Static);

                method.Invoke(null, null);
            };
        }
    }
}
