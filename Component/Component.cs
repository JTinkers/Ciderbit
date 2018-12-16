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

            //The omnipotent all-catcher
            try
            {
                Enable();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void Enable()
        {
            #region behaviour of modules
            Compiler.CompilationFailed += (o, e) =>
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

                Script script = new Script();

                if (e.Payload.Type == PayloadType.Code)
                {
                    Console.WriteLine($"#\tComponent: code from the payload will now be compiled.");

                    script.Assembly = Compiler.Create(Encoding.Default.GetString(e.Payload.Data));
                }

                if (e.Payload.Type == PayloadType.Files)
                {
                    Console.WriteLine($"#\tComponent: files pointed to by the payload will now be compiled.");

                    script.Info = ScriptInfo.Deserialize(e.Payload.Info);
                    script.Assembly = Compiler.Create(e.Payload.GetFiles(), script.Info.LinkedAssemblies.Split(';'));
                }

                if (script.Assembly == null)
                {
                    Console.WriteLine($"#\tComponent: something went wrong - script object is null.");
                    return;
                }

                Console.WriteLine($"#\tComponent: executing {script.Info.Name} by {script.Info.Author}.");

                script.Execute();
            };
            #endregion

            Conduit.Open();
        }
    }
}
