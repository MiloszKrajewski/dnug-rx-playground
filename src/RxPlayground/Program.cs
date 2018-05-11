using System;
using System.IO;

namespace RxPlayground
{
	class Program
	{
		static void Main(string[] args)
		{
			Logging.ConfigureLogging(Path.GetTempPath(), "RxPlayground");

			var logger = Logging.For("Main");
			logger.Fatal("Fatal");
			logger.Error("Error");
			logger.Warn("Warning");
			logger.Info("Information");
			logger.Debug("Debug");
			logger.Trace("Traace");

			Console.ReadLine();
		}
	}
}
