using System.IO;

namespace Sem1
{
    public static class ElementIO
    {
        /// <summary>
        /// Reads ElementList from a file.
        /// </summary>
        public static ElementList Read(string path)
        {
            Element first = null;
            Element current = null;
            var reader = File.OpenText(path);
            while (!reader.EndOfStream)
            {
                var s = reader.ReadLine();
                if (first == null)
                {
                    first = new Element(Segment.Parse(s));
                    current = first;
                }
                else
                {
                    current.Next = new Element(Segment.Parse(s));
                    current = current.Next;
                }
            }
            reader.Close();
            return new ElementList(first);
        }
        
        /// <summary>
        /// Writes ElementList to file.
        /// </summary>
        public static void Write(ElementList e, string path)
        {
            //var current = e.First;
            var writer = File.CreateText(path);
            //while (current != null)
            //{
            //    writer.WriteLine(current.Value.ToString());
            //    current = current.Next;
            //}
            writer.Write(e.ToString());
            writer.Close();
        }
    }
}