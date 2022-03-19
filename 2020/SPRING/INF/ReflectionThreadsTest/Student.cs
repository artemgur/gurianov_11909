using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReflectionThreadsTest
{
    public class Student
    {
        public readonly string LastName;
        public readonly Task[] Tasks;

        private Student(string lastName, Task[] tasks)
        {
            LastName = lastName;
            Tasks = tasks;
        }

        public static void ReadStudents()
        {
            TestSystem.Students = new List<Student>();
            foreach (var file in Directory.EnumerateFiles(Paths.Variants))
            {
                var contents = File.ReadAllLines(file);
                var name = Path.GetFileName(file).Split('.').First();
                var tasks = contents.Select(x => TestSystem.Tasks[x]).ToArray();
                TestSystem.Students.Add(new Student(name, tasks));
            }
        }

        public void Check()
        {
            var path = Paths.Students + LastName;
            var result = new Dictionary<string, Dictionary<string, TestResult>>();
            foreach (var file in Directory.EnumerateFiles(path))
            {
                var dict = new Dictionary<string, TestResult>();
                var info = new FileInfo(file);
                var assembly = Assembly.LoadFile(info.FullName);
                var taskId = Path.GetFileName(file).Split('.').First();
                var task = TestSystem.Tasks[taskId];
                //var type = assembly.GetTypes().SingleOrDefault(x => x.Name == task.Class);
                var type = assembly.GetTypes().SingleOrDefault(x => x.Name == task.Class);
                var method = type?.GetMethod(task.Method);
                var obj = type?.GetConstructor(new Type[] { })?.Invoke(new object[]{});
                if (type == null || method == null || obj == null)
                {
                    foreach (var test in task.Tests)
                        dict.Add(test.Name, TestResult.NoClassOrMethod);
                    result.Add(taskId, dict);
                    continue;
                }
                foreach (var test in task.Tests)
                {
                    object res;
                    try
                    {
                        res = method.Invoke(obj, test.Parameters);
                    }
                    catch (Exception)
                    {
                        dict.Add(test.Name, TestResult.Exception);
                        result.Add(taskId, dict);
                        continue;
                    }
                    if (res.Equals(test.ExpectedResult))
                        dict.Add(test.Name, TestResult.Passed);
                    else
                        dict.Add(test.Name, TestResult.WrongResult);
                }
                result.Add(taskId, dict);
            }
            foreach (var task in Tasks)
                if (!result.ContainsKey(task.Id))
                {
                    var dict = new Dictionary<string, TestResult>();
                    foreach (var test in task.Tests)
                    {
                        dict.Add(test.Name, TestResult.NotDone);
                    }
                    result.Add(task.Id, dict);
                }
            //return result;
            TestSystem.Results.TryAdd(LastName, result);
        }
    }
}