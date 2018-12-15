using System;
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
                Console.WriteLine($"#\tConduit: data received:\n\t{Encoding.Default.GetString(e.Data)}");
            };

            var code = @"using System;
                namespace Ciderbit
                {
                    class Script
                    {
                        public void Main()
                        {
                            Console.WriteLine(" + '"' + "YES" + '"' + @");
                        }
                    }
                }";

            var script = Scripter.Compile(code).CreateInstance("Ciderbit.Script");
            script.GetType().GetMethod("Main").Invoke(script, null);
        }
    }
}
