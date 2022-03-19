using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace AlgorithmsData
{
    public class RadixSortEntry
    {
        public readonly string Value;
        public RadixSortEntry Next;

        public RadixSortEntry(string value)
        {
            Value = value;
        }
    }
    
    public static class RadixSort
    {
        private static SortedDictionary<char, RadixSortEntry> pocketsStart;
        private static SortedDictionary<char, RadixSortEntry> pocketsEnd;
        private static int length;
        private static RadixSortEntry start;
        
        public static List<string> Sort(List<string> original)
        {
            start = Convert(original);
            for (var i = length - 1; i >= 0; i--)
            {
                SortRadix(i);
            }
            return ConvertBack();
        }

        private static List<string> ConvertBack()
        {
            var r = new List<string>();
            var current = start;
            while (current != null)
            {
                r.Add(current.Value);
                current = current.Next;
            }
            return r;
        }

        private static RadixSortEntry Convert(List<string> original)
        {
            var r = new RadixSortEntry(original[0]);
            length = original[0].Length;//Temporary
            var current = r;
            for (var i = 1; i < original.Count; i++)
            {
                var p = new RadixSortEntry(original[i]);
                current.Next = p;
                current = p;
            }
            return r;
        }

        private static void SortRadix(int i)
        {
            pocketsStart = new SortedDictionary<char, RadixSortEntry>();
            pocketsEnd = new SortedDictionary<char, RadixSortEntry>();
            var current = start;
            while (current != null)
            {
                var key = current.Value[i];
                if (pocketsStart.ContainsKey(key))
                {
                    pocketsEnd[key].Next = current;
                    pocketsEnd[key] = current;
                    //current.Next = null;
                }
                else
                {
                    pocketsStart.Add(key, current);
                    pocketsEnd.Add(key, current);
                    //current.Next = null;
                }
                current = current.Next;
            }
            var keys = pocketsStart.Keys.ToArray();
            start = pocketsStart[keys[0]];
            current = pocketsEnd[keys[0]];
            for (var j = 1; j < keys.Length; j++)
            {
                current.Next = pocketsStart[keys[j]];
                current = pocketsEnd[keys[j]];
            }
            //current = current.Next;
            current.Next = null;
        }
    }

    [TestFixture]
    class RadixSortTests
    {
        private static List<string> GenerateRandomStringList(int length, int number)
        {
            var r = new List<string>();
            var random = new Random();
            for (var i = 0; i < number; i++)
            {
                var builder = new StringBuilder();
                for (var j = 0; j < length; j++)
                {
                    builder.Append((char)random.Next((int)'0', (int)'9' + 1));
                }
                r.Add(builder.ToString());
            }
            return r;
        }

        [Test]
        public static void RadixSortTest()
        {
            var r = GenerateRandomStringList(6, 4);
            var t = RadixSort.Sort(r);
            r.Sort();
            for (var i = 0; i < r.Count; i++)
            {
                Assert.AreEqual(r[i],t[i]);
            }
        }
    }
}