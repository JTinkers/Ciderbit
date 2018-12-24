using System;
using System.IO;
using System.Reflection;

namespace TestAssembly
{
	public static class TestEntryPoint
	{
		public static void Main(string[] args)
		{
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolver);

			Print();

			Console.ReadKey(true);
		}

		public static void Print()
		{
			TestReferenceLib.TestReferenceClass.PrintOut();
		}

		private static Assembly AssemblyResolver(object sender, ResolveEventArgs args)
		{
			var memory = new MemoryStream();

			Assembly.GetEntryAssembly().GetManifestResourceStream(args.Name.Split(',')[0] + ".dll").CopyTo(memory);

			var assembly = AppDomain.CurrentDomain.Load(memory.ToArray());

			return assembly ?? null;
		}
	}
}
