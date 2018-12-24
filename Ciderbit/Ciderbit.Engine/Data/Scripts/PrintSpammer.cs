using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace PrintSpammerNamespace
{
	public delegate void _printOut();
	public class PrintSpammer
	{
		// Required for infinite loops to prevent AppDomain lock when attempting to unload it.
		public static bool IsUnloading = false;

		public static void Main(string[] args)
		{
			// Required for infinite loops to prevent AppDomain lock when attempting to unload it.
			AppDomain.CurrentDomain.DomainUnload += new EventHandler((o, e) => IsUnloading = true);

			var printOutMethod = Marshal.GetDelegateForFunctionPointer<_printOut>(new IntPtr(0x00E9141F));
			printOutMethod();
		}
	}
}
