using System.IO;

namespace Services
{
	public class FileLogger:ILogger
	{
		public const string FileName = "log.txt";
		
		public void Log(string input)
		{
			File.AppendAllText(FileName, input);
		}
	}
}