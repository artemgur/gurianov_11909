using System;
using System.Collections.Generic;

public static class EnumerableFunctions
{
    public static IEnumerable<T> Generate<T>(Func<T> makeNext)
    {
        var current = makeNext();
        while (current != null)
        {
            yield return current;
            current = makeNext();
        }
    }

    public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> source, Func<T, bool> shouldStop)
    {
        foreach (var x in source)
            if (!shouldStop(x))
                yield return x;
    }

    public static IEnumerable<string> ReadUntilEmpty() =>
        TakeUntil(Generate(Console.ReadLine), x => x == "");
}