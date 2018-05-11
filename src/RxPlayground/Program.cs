using System;
using System.IO;

namespace RxPlayground
{
	class Program
	{
		static void Main(string[] args)
		{
			Logging.ConfigureLogging(Path.GetTempPath(), "RxPlayground");
			var log = Logging.For("+");

			Console.ReadLine();
		}
	}
}
