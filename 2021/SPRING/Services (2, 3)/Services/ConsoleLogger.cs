using System;

namespace Services
{
	public class ConsoleLogger:ILogger
	{
		public void Log(string input)
		{
			Console.WriteLine(input);
		}
	}
}