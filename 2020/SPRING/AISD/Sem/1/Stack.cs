namespace Sem1
{
    public class Stack
    {
        private Element first;

        public void Push(Element e)
        {
            e.NextInStack = first;
            first = e;
        }

        public Element Pop()
        {
            if (first == null)
                return null;
            var old = first;
            first = old.NextInStack;
            return old;
        }
    }
}