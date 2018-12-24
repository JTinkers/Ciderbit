using System;
using System.Threading;

namespace PrintSpammerNamespace
{
	public class PrintSpammer
	{
		public static void Main(string[] args)
		{
			for(int i = 0; i<3; i++)
			{
				Console.WriteLine("SPAM!");

				Thread.Sleep(1000);
			}
		}
	}
}
