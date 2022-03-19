namespace ReflectionThreadsTest
{
    public class Test
    {
        public readonly string Name;
        public readonly object[] Parameters;
        public readonly object ExpectedResult;

        public Test(string name, object[] parameters, object expectedResult)
        {
            Name = name;
            Parameters = parameters;
            ExpectedResult = expectedResult;
        }
    }
}