using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ReflectionThreadsTest
{
    public static class TestSystem
    {
        private const int ColumnWidth = 20;
        
        public static Dictionary<string, Task> Tasks;//TODO Maybe move it somewhere later
        public static List<Student> Students;
        public static ConcurrentDictionary<string, Dictionary<string, Dictionary<string, TestResult>>> Results;

        public static void Initialize()
        {
            Tasks = new Dictionary<string, Task>();
            Task.ReadTasks();
            Student.ReadStudents();
        }

        public static void Run(int threadsCount)
        {
            Results = new ConcurrentDictionary<string, Dictionary<string, Dictionary<string, TestResult>>>(threadsCount, Students.Count);
            var studentsForThread = Students.Count / threadsCount;
            var studentsLeft = Students.Count - studentsForThread * threadsCount;
            var threads = new Thread[threadsCount];
            var first = 0;
            for (var i = 0; i < threadsCount; i++)
            {
                if (i < studentsLeft)
                {
                    var first1 = first;
                    threads[i] = new Thread(() => CheckStudentsFromRange(first1, first1 + studentsForThread));
                    first = first + studentsForThread + 1;
                }
                else
                {
                    var first1 = first;
                    threads[i] = new Thread(() => CheckStudentsFromRange(first1, first1 + studentsForThread - 1));
                    first = first + studentsForThread;
                }
                threads[i].Start();
            }
            while (Results.Count != Students.Count)
                Thread.Sleep(50);
            WriteResults();
        }

        private static void WriteResults()
        {
            foreach (var student in Results)
            {
                var builder = new StringBuilder();
                builder.Append(String.Format("{0,20}", student.Key));
                foreach (var task in student.Value)
                {
                    builder.Append(String.Format("{0,20}", task.Key));
                    foreach (var test in task.Value)
                    {
                        builder.Append(String.Format("{0,20}", test.Key));
                        builder.Append(String.Format("{0,20}", test.Value));
                        Console.WriteLine(builder.ToString());
                        builder.Remove(builder.Length - ColumnWidth * 2, ColumnWidth * 2);
                    }
                    builder.Remove(builder.Length - ColumnWidth, ColumnWidth);
                }
            }
        }

        private static void CheckStudentsFromRange(int first, int last)
        {
            if (first > last)
                return;
            for (var i = first; i <= last; i++)
                Students[i].Check();
        }
    }
}