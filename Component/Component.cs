using System;
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
        }
    }
}
