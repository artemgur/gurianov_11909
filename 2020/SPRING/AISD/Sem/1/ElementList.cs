using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using static Sem1.SegmentConditions;

namespace Sem1
{
    /// <summary>
    /// Singly linked list.
    /// </summary>
    public class ElementList: IEnumerable
    {
        internal Element First;

        public ElementList(Element first)
        {
            First = first;
        }

        public Segment this[int i]
        {
            get => Get(i);
            set => GetElement(i).Value = value;
        }
        
        public IEnumerator<Segment> GetEnumerator()
        {
            var current = First;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
        
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Inserts segment s to a position.
        /// </summary>
        public void Insert(Segment s, int position)
        {
            if (position == 0)
            {
                var e = new Element(s);
                e.Next = First;
                First = e;
                return;
            }
            var current = First;
            for (var i = 0; i < position - 1; i++)
                current = current.Next;
            current.AddNext(new Element(s));
        }

        /// <summary>
        /// Removes segment s from a position.
        /// </summary>
        public void Remove(int position)
        {
            if (position == 0)
            {
                First = First.Next;
                return;
            }

            var current = First;
            for (var i = 0; i < position - 1; i++)
                current = current.Next;
            current.RemoveNext();
        }

        private Segment Get(int position)
        {
            var current = First;
            for (var i = 0; i < position; i++)
                current = current.Next;
            return current.Value;
        }
        
        private Element GetElement(int position)
        {
            var current = First;
            for (var i = 0; i < position; i++)
                current = current.Next;
            return current;
        }

        /// <summary>
        /// Converts ElementList to list.
        /// </summary>
        public List ToList()
        {
            var current = First;
            var list = new List();
            while (current != null)
            {
                list.Add(current.Value);
                current = current.Next;
            }

            return list;
        }

        /// <summary>
        /// Creates new ElementList, which is sorted by length of segments.
        /// </summary>
        public ElementList CreateListSortedByLength()
        {
            var l = ToList();
            l.Sort(new LengthComparer());
            return FromList(l);
        }

        /// <summary>
        /// Creates ElementList from list.
        /// </summary>
        public static ElementList FromList(List list)
        {
            if (list.Count == 0)
                throw new ArgumentException("List length can't be 0");
            var res = new ElementList(new Element(list[0]));
            var current = res.First;
            for (var i = 1; i < list.Count; i++)
            {
                current.Next = new Element(list[i]);
                current = current.Next;
            }

            return res;
        }

        public ElementList CreatePi3Pi6Sequence() => CreateNewElementList(PI3PI6Condition);
        
        public ElementList CreateLengthRangeSequence(params double[] lengthRange) => CreateNewElementList(LengthRangeCondition, lengthRange);
        
        public string GetInvertedString()
        {
            var current = First;
            var s = new Stack();
            while (current != null)
            {
                s.Push(current);
                current = current.Next;
            }
            var builder = new StringBuilder();
            var e = s.Pop();
            while (e != null)
            {
                builder.Append(e.Value.ToString());
                builder.Append("\r\n");
                e = s.Pop();
            }

            return builder.ToString();
        }

        /// <summary>
        /// Creates a new list of elements, for which the delegate "condition" returns true.
        /// </summary>
        public ElementList CreateNewElementList(SegmentCondition condition, params double[] additionalData)
        {
            var currentOld = First;
            var res = new ElementList(null);
            //Element first = null;
            Element currentNew = null;
            while (currentOld != null)
            {
                if (condition(currentOld.Value, additionalData))
                    if (res.First == null)
                    {
                        res.First = currentOld.Copy();
                        currentNew = res.First;
                    }
                    else
                    {
                        currentNew.Next = currentOld.Copy();
                        currentNew = currentNew.Next;
                    }

                currentOld = currentOld.Next;
            }

            return res;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var v in this)
            {
                builder.Append(v);
                builder.Append("\r\n");
            }
            return builder.ToString();
        }

    }
}