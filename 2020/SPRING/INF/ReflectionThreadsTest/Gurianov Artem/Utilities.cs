using System;
using System.Collections.Generic;

namespace ReflectionThreadsTest
{
    public static class Utilities
    {
        private static readonly Dictionary<string, Type> SystemAliases = new Dictionary<string, Type>
        {
            {"bool",      typeof(Boolean)},
            {"byte",      typeof(Byte)},
            {"sbyte",     typeof(SByte)},
            {"char",      typeof(Char)},
            {"decimal",   typeof(Decimal)},
            {"double",    typeof(Double)},
            {"float",     typeof(Single)},
            {"int",       typeof(Int32)},
            {"uint",      typeof(UInt32)},
            {"long",      typeof(Int64)},
            {"ulong",     typeof(UInt64)},
            {"object",    typeof(Object)},
            {"short",     typeof(Int16)},
            {"ushort",    typeof(UInt16)},
            {"string",    typeof(String)}
        };

        public static Type ParseType(string str)
        {
            if (SystemAliases.ContainsKey(str))
                return SystemAliases[str];
            return Type.GetType(str);
        }
        
        public static object Parse(string str, Type type)
        {
            if (type == typeof(String))
                return str;
            return type.GetMethod("Parse", new []{typeof(String)}).Invoke(null, new object[] {str});
        }

        /*
        public static string ToString(this TestResult result)
        {
            switch (result)
            {
                case TestResult.Passed:
                    break;
                case TestResult.WrongResult:
                    break;
                case TestResult.NoClassOrMethod:
                    break;
                case TestResult.Exception:
                    break;
                case TestResult.NotDone:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }
        }
    */
    }
}