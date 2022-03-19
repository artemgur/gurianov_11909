using System;

namespace Sem1
{
    class Program
    {
        static void Main()
        {
            var list = ElementIO.Read("main.txt");//TODO: Create file
            Console.WriteLine("Original list (read from \"main.txt\"):");
            Console.WriteLine(list);
            Console.WriteLine("List of segments with PI/4 and PI/6 angle");
            Console.WriteLine(list.CreatePi3Pi6Sequence().ToString());
            Console.WriteLine("List of segments with length between 9 and 21:");
            Console.WriteLine(list.CreateLengthRangeSequence(9, 21));
            Console.WriteLine("Inverted list:");
            Console.WriteLine(list.GetInvertedString());
            list.Insert(new Segment(42, 7, 4, 8), 3);
            Console.WriteLine("List after insertion of new element:");
            Console.WriteLine(list);
            list.Remove(2);
            Console.WriteLine("List after removal of an element:");
            Console.WriteLine(list);
            var sorted =  list.CreateListSortedByLength();
            Console.WriteLine("Sorted list:");
            Console.WriteLine(sorted);
            ElementIO.Write(sorted, "mainOut.txt");
            Console.WriteLine("Sorted list was written to \"mainOut.txt\".");
        }
    }
}