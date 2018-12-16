using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Ciderbit.Libraries;
using Ciderbit.Types;

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

            #region behaviour of modules
            Scripter.CompilationFailed += (o, e) =>
            {
                Console.WriteLine("#\tScripter: compile errors occured:\n");

                Console.ForegroundColor = ConsoleColor.Red;

                foreach (CompilerError error in e.Errors)
                    Console.WriteLine($"\t[Line {error.Line}]:\t{error.ErrorText}");

                Console.ForegroundColor = ConsoleColor.DarkCyan;
            };

            Conduit.ClientConnected += (o, e) => Console.WriteLine("#\tConduit: client connected.");

            Conduit.DataReceived += (o, e) =>
            {
                Console.WriteLine($"#\tConduit: data payload received of size: [{e.Payload.Data.Length} bytes] and type: [{e.Payload.Type}].");

                Assembly script = null;

                if (e.Payload.Type == PayloadType.Code)
                {
                    Console.WriteLine($"#\tComponent: code from the payload will now be compiled and executed.");

                    script = Scripter.Compile(Encoding.Default.GetString(e.Payload.Data));
                }

                if (e.Payload.Type == PayloadType.Files)
                {
                    Console.WriteLine($"#\tComponent: files pointed to by the payload will now be compiled and executed.");

                    script = Scripter.Compile(e.Payload.GetFiles());
                }

                var method = script.GetType("Ciderbit.Script", true, false).GetMethod("Start", BindingFlags.Public | BindingFlags.Static);

                method.Invoke(null, null);
            };
            #endregion

            Conduit.Open();
        }
    }
}
