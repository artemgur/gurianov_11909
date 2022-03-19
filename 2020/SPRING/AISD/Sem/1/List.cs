using System;
using System.Collections.Generic;

namespace Sem1
{
    public class List
    {
        private Segment[] array;
        private const int StartCapacity = 10;
        private int FirstEmptyIndex;

        public int Count => FirstEmptyIndex;

        public List()
        {
            array = new Segment[StartCapacity];
            FirstEmptyIndex = 0;
        }

        public List(double[][] l)
        {
            array = new Segment[l.GetLength(0)];
            for (var i = 0; i < array.Length; i++)
                array[i] = new Segment(l[i][0], l[i][1], l[i][2], l[i][3]);
        }
        
        public void Add(Segment s)
        {
            if (FirstEmptyIndex < array.Length)
                array[FirstEmptyIndex] = s;
            else
            {
                var old = array;
                Array.Copy(old, array, array.Length * 2);
            }
            FirstEmptyIndex++;
        }

        public void Sort(IComparer<Segment> comparer)
        {
            Segment[] arr = new Segment[Count];
            Array.Copy(array, arr, Count);
            Array.Sort(arr, comparer);
            array = arr;
        }
        
        public Segment this[int i]
        {
            get => array[i];
            set => array[i] = value;
        }
    }
}