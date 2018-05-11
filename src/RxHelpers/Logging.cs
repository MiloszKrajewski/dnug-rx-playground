using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;

// ReSharper disable once CheckNamespace
namespace System
{
	[Flags]
	public enum Targets
	{
		Console = 0x01,
		File = 0x02,
		Default = Console | File,
		All = Console | File,
	}

	public class Logging
	{
		public static Logger For(string name) => LogManager.LogFactory.GetLogger(name);

		public static void ConfigureLogging(
			string folder, string productName, Targets targets = Targets.Default)
		{
			var config = new LoggingConfiguration();
			if ((targets & Targets.Console) != 0)
				ConfigureConsoleLogging(config);
			if ((targets & Targets.File) != 0)
				ConfigureFileLogging(config, folder, productName);
			LogManager.Configuration = config;
		}

		private static void ConfigureFileLogging(
			LoggingConfiguration config, string folder, string productName)
		{
			Directory.CreateDirectory(folder);

			var target = new FileTarget {
				Name = "file",
				Layout = "${date:format=yyyyMMdd.HHmmss} ${threadid}> [${level}] (${logger}) ${message}",
				ArchiveEvery = FileArchivePeriod.Day,
				MaxArchiveFiles = 7,
				FileName = Path.Combine(folder, productName + ".log"),
				ArchiveFileName = Path.Combine(folder, productName + ".bak"),
			};
			config.AddTarget("file", target);
			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, target));
		}

		private static void ConfigureConsoleLogging(LoggingConfiguration config)
		{
			var console = new ColoredConsoleTarget {
				Name = "console",
				Layout = "${threadid}> (${logger}) ${message}",
				UseDefaultRowHighlightingRules = true,
				ErrorStream = false,
			};

			void Configure(LogLevel level, ConsoleOutputColor color) =>
				console.RowHighlightingRules.Add(
					new ConsoleRowHighlightingRule(
						$"level == LogLevel.{level}",
						color,
						ConsoleOutputColor.NoChange));

			Configure(LogLevel.Trace, ConsoleOutputColor.DarkGray);
			Configure(LogLevel.Debug, ConsoleOutputColor.Gray);
			Configure(LogLevel.Info, ConsoleOutputColor.Cyan);
			Configure(LogLevel.Warn, ConsoleOutputColor.Yellow);
			Configure(LogLevel.Error, ConsoleOutputColor.Red);
			Configure(LogLevel.Fatal, ConsoleOutputColor.Magenta);

			config.AddTarget("console", console);

			config.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, console));
		}
	}
}
