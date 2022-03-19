using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Benchmarking
{
    public class BoyerMooreBenchmark: ITask
    {
        private static int patternSize = 10;
        public const int startLength = 20;
        public const int endLength = 200;
        public const int step = 20;
        private const int dataNForInput = 30;

        private string source;
        private string pattern;

        public BoyerMooreBenchmark(string source, string pattern)
        {
            this.source = source;
            this.pattern = pattern;
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        public void Run()
        {
            source.Run(pattern);
        }
        
        ///Генерация случайной строки длины length 
        public static string RandomString(int length)
        {
            var builder = new StringBuilder();
            var random = new Random();
            for (var i = 0; i < length; i++)
                builder.Append(random.Next('a', 'z' + 1));
            return builder.ToString();
        }

        ///Генерация случайных данных 
        public static void GenerateData()
        {
            var writer = File.CreateText("input.txt");
            for (var i = startLength; i <= endLength; i += step)
            {
                for (var j = 0; j < dataNForInput; j++)
                {
                    var pattern = RandomString(patternSize);
                    var source = RandomString(i);
                    while (source.Run(pattern) != -1)
                    {
                        pattern = RandomString(patternSize);
                    }
                    writer.WriteLine(source);
                    writer.WriteLine(pattern);
                }
            }
            writer.Close();
        }

        ///Генерация данных для графика
        public static ValueTuple<ChartData, ChartData> ChartData(IterationsBenchmark benchmark, int repetitionsCount)
        {
            var results = new List<ExperimentResult>();
            var iterResults = new List<ExperimentResult>();
            var writer = File.CreateText("BenchmarkData.txt");
            var reader = File.OpenText("input.txt");
            for (var i = startLength; i <= endLength; i+=step)
            {
                for (var j = 0; j < dataNForInput; j++)
                {
                    var task = new BoyerMooreBenchmark(reader.ReadLine(), reader.ReadLine());
                    var r = benchmark.MeasureDurationInMs(task, repetitionsCount);
                    var iter = benchmark.IterationsResult;
                    var result = new ExperimentResult(i, r);
                    var iterResult = new ExperimentResult(i, iter);
                    //writer.WriteLine(@"{0, 3} {1, 5}", i, r);
                    results.Add(result);
                    iterResults.Add(iterResult);
                }
            }
            results = results.GroupBy(z => z.InputSize).Select(z =>
                z.Aggregate((q, w) => new ExperimentResult(q.InputSize, q.AverageResult + w.AverageResult)))
                .Select(z => {z.AverageResult /= dataNForInput; return z; }).ToList();
            iterResults = iterResults.GroupBy(z => z.InputSize).Select(z =>
                z.Aggregate((q, w) => new ExperimentResult(q.InputSize, q.AverageResult + w.AverageResult)))
                .Select(z => {z.AverageResult /= dataNForInput; return z; }).ToList();
            for (var i = 0; i < results.Count; i++)
                writer.WriteLine("{0, 3} {1:0.0000} {2:0.0}", results[i].InputSize, results[i].AverageResult, iterResults[i].AverageResult);
            writer.Close();
            reader.Close();
            var x = new ChartData
            {
                CurveLabel = "Алгоритм Бойера — Мура (время)",
                YAxisLabel = "Time",
                Results = results
            };
            var y = new ChartData
            {
                CurveLabel = "Алгоритм Бойера — Мура (итерации)",
                YAxisLabel = "Iterations",
                Results = iterResults
            };
            return ValueTuple.Create(x, y);
        }
    }
     
    ///Вычисляет производительность и количество итераций
    public class IterationsBenchmark : IBenchmark
    {
        public double IterationsResult;
            
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            task.Run();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            BoyerMooreAlgorithm.iterationsCounter = 0;
            for (var i = 0; i < repetitionCount; i++)
                task.Run();
            stopwatch.Stop();
            IterationsResult = (double)BoyerMooreAlgorithm.iterationsCounter / repetitionCount;
            return (double) stopwatch.ElapsedMilliseconds / repetitionCount;
        }
    }
}