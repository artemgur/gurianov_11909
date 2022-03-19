using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReflectionThreadsTest
{
    public class Task
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Class;
        public readonly string Method;
        public readonly string[] Parameters;
        public readonly Type[] ParameterTypes;
        public readonly Type ReturnType;
        public List<Test> Tests { get; private set; }

        private Task(string id, string name, string className, string method, string[] parameters, Type[] parameterTypes, Type returnType)
        {
            Id = id;
            Name = name;
            Class = className;
            Method = method;
            Parameters = parameters;
            ParameterTypes = parameterTypes;
            ReturnType = returnType;
        }

        public static void ReadTasks()
        {
            foreach (var file in Directory.EnumerateFiles(Paths.Tasks))
            {
                var id = Path.GetFileName(file).Split('.').First();
                var contents = File.ReadAllLines(file);
                var str1 = contents[1].Split('.');
                var parameters = new string[contents.Length - 3];//First 3 lines are occupied by another data
                var parameterTypes = new Type[contents.Length - 3];//First 3 lines are occupied by another data
                for (var i = 3; i < contents.Length; i++)
                {
                    var splitRes = contents[i].Split(':');
                    parameters[i - 3] = splitRes[0];
                    parameterTypes[i - 3] = Utilities.ParseType(splitRes[1]);
                }
                var task = new Task(id, contents[0], str1[0], str1[1], parameters, parameterTypes, Utilities.ParseType(contents[2]));
                task.ReadTests();
                TestSystem.Tasks.Add(id, task);
            }
        }

        private void ReadTests()
        {
            Tests = new List<Test>();
            var testPath = Paths.Tests + Id;
            foreach (var file in Directory.EnumerateFiles(testPath))
            {
                var name = Path.GetFileName(file).Split('.').First();
                var contents = File.ReadAllLines(file);
                //if (contents.Length != Parameters.Length + 1)
                //    throw new ArgumentException();
                var parameters = new object[Parameters.Length];
                for (var i = 0; i < contents.Length - 1; i++) //Last line of the file is occupied by expected return 
                {
                    var splitRes = contents[i].Split(':');
                    var param = splitRes[0];
                    var paramValue = splitRes[1];//Probably we don't really need parameter name
                    var index = Array.IndexOf(Parameters, param);
                    if (index == -1)
                        throw new ArgumentException();
                    parameters[index] = Utilities.Parse(paramValue, ParameterTypes[i]);
                }
                Tests.Add(new Test(name, parameters, Utilities.Parse(contents[contents.Length - 1], ReturnType)));
            }
        }
    }
}