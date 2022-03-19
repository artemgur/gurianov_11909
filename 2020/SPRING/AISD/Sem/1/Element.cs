using System;

namespace Sem1
{
    public class Element
    {
        public Segment Value;
        public Element Next;
        public Element NextInStack;
        
        public void RemoveNext()
        {
            if (Next != null)
                Next = Next.Next;
            else
                throw new InvalidOperationException("Next element doesn't exist");
        }

        public void AddNext(Element e)
        {
            e.Next = Next;
            Next = e;
        }

        public Element(Segment value) => Value = value;
        
        ///Creates a copy of an object. Doesn't copy the link to next object.
        public Element Copy()
        {
            return new Element(Value);
        }
    }
}