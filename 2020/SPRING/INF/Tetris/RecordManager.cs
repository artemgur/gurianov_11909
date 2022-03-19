using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tetris
{
    public static class RecordManager
    {
        public static string ReadRecords()
        {
            if (!File.Exists(Constants.RecordsFilename))
                CreateEmptyRecordFile();
            return File.ReadAllText(Constants.RecordsFilename);
        }

        private static void CreateEmptyRecordFile()
        {
            var x = File.Create(Constants.RecordsFilename);
            x.Close();
            WriteRecords(Enumerable.Repeat(0, 10));
        }

        public static void WriteRecords(IEnumerable<int> records)
            => File.WriteAllLines(Constants.RecordsFilename, records.Select(x => x.ToString()));

        public static void AddResult(int result)
        {
            WriteRecords(File.ReadAllLines(Constants.RecordsFilename)
                .Select(int.Parse).Prepend(result)
                .OrderByDescending(x => x).Take(Constants.RecordNumber));
        }
    }
}