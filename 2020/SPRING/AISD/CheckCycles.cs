using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AlgorithmsData
{
    public class SequenceElement<T>
    {
        public int value;
        public SequenceElement<T> next;

        public SequenceElement(int value)
        {
            this.value = value;
        }
    }
    
    public class CheckCycles
    {
        public static bool Check<T>(SequenceElement<T> first)
        {
            var set = new HashSet<SequenceElement<T>>();
            var current = first;
            while (current != null)
                if (set.Contains(current))
                    return true;
                else
                {
                    set.Add(current);
                    current = current.next;
                }
            return false;
        }
    }
    
    [TestFixture]
    class CheckCyclesTests
    {
        private const int MinL = 2;
        private const int MaxL = 6;
        
        [Test]
        public void CycleTest()
        {
            var first = GenerateSequence(true);
            var t = CheckCycles.Check(first);
            Assert.True(CheckCycles.Check(first));
        }

        [Test]
        public void NotCycleTest()
        {
            var first = GenerateSequence(false);
            Assert.False(CheckCycles.Check(first));
        }
        
        private SequenceElement<int> GenerateSequence(bool cycled)
        {
            var loop = -1;
            var loopTo = -1;
            var random = new Random();
            var length = random.Next(MinL, MaxL + 1);
            if (cycled)
            {
                loop = random.Next(0, length);
                loopTo = random.Next(0, loop + 1);
            }
            var elements = new SequenceElement<int>[length + 1];
            SequenceElement<int> first = new SequenceElement<int>(0);
            elements[0] = first;
            var current = first;
            for (var i = 0; i < length; i++)
            {
                if (cycled && (loop == i))
                {
                    current.next = elements[loopTo];
                    break;
                }
                current.next = new SequenceElement<int>(i + 1);
                elements[i + 1] = current;
                current = current.next;
            }
            return first;
        }
    }
}