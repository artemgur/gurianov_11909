using System;
using System.IO;
using NUnit.Framework;

namespace Sem1
{
    [TestFixture]
    class Tests
    {
        [TestCase("inputTest.txt")]
        public void ReadTest(string path)
        {
            var inp = ElementIO.Read(path);
            Assert.AreEqual(new Segment(1,2,3,4), inp[0]);
            Assert.AreEqual(new Segment(5,6,7,8), inp[1]);
            Assert.AreEqual(new Segment(0,0,0,0), inp[2]);
        }

        [TestCase("outputTest1.txt")]
        public void WriteTest(string path)
        {
            var e1 = new Element(new Segment(1, 1, 1, 1));
            e1.Next = new Element(new Segment(2, 2, 2, 2));
            e1.Next.Next = new Element(new Segment(3, 4, 5, 6));
            var e = new ElementList(e1);
            ElementIO.Write(e, path);
            var reader = File.OpenText(path);
            var s = reader.ReadToEnd();
            Assert.AreEqual("1 1 1 1\r\n2 2 2 2\r\n3 4 5 6\r\n", s);
        }
        
        [Test]
        public void InsertTest()
        {
            var e1 = new Element(new Segment(1, 1, 1, 1));
            e1.Next = new Element(new Segment(2, 2, 2, 2));
            e1.Next.Next = new Element(new Segment(3, 4, 5, 6));
            var e = new ElementList(e1);
            e.Insert(new Segment(0, 0, 0, 0), 2);
            Assert.AreEqual(new Segment(0, 0, 0, 0), e[2]);
        }
        
        [Test]
        public void RemoveTest()
        {
            var e1 = new Element(new Segment(1, 1, 1, 1));
            e1.Next = new Element(new Segment(2, 2, 2, 2));
            e1.Next.Next = new Element(new Segment(0, 0, 0, 0));
            e1.Next.Next.Next = new Element(new Segment(3, 4, 5, 6));
            var e = new ElementList(e1);
            e.Remove(2);
            Assert.AreEqual(new Segment(3, 4, 5, 6), e[2]);
        }
        
        [Test]
        public void Pi4Pi6SequenceTest()
        {
            var e1 = new Element(new Segment(0, 0, 1, 1));
            e1.Next = new Element(new Segment(2, 2, 23, 28));
            e1.Next.Next = new Element(new Segment(0, 0, Math.Sqrt(3), 1));
            e1.Next.Next.Next = new Element(new Segment(3, 4, -5, -6));
            var e = new ElementList(e1);
            var res = e.CreatePi3Pi6Sequence();
            Assert.AreEqual(new Segment(0, 0, 1, 1), res[0]);
            Assert.AreEqual(new Segment(0, 0, Math.Sqrt(3), 1), res[1]);
        }
        
        [Test]
        public void InverseTest()
        {
            var e1 = new Element(new Segment(3, 4, 5, 6));
            e1.Next = new Element(new Segment(2, 2, 2, 2));
            e1.Next.Next = new Element(new Segment(1, 1, 1, 1));
            var e = new ElementList(e1);
            var s = e.GetInvertedString();
            Assert.AreEqual("1 1 1 1\r\n2 2 2 2\r\n3 4 5 6\r\n", s);
        }
        
        [Test]
        public void IntervalTest()
        {
            var e1 = new Element(new Segment(0, 0, 1, 0));
            e1.Next = new Element(new Segment(0, 0, 200, 200));
            e1.Next.Next = new Element(new Segment(0, 0, 10, 1));
            var e = new ElementList(e1);
            var res = e.CreateLengthRangeSequence(2, 15);
            Assert.AreEqual(new Segment(0, 0, 10, 1), res[0]);
        }

        [Test]
        public void SortTest()
        {
            var e1 = new Element(new Segment(0, 0, 10, 10));
            e1.Next = new Element(new Segment(0, 0, 2, 2));
            e1.Next.Next = new Element(new Segment(0, 0, 0, 0));
            e1.Next.Next.Next = new Element(new Segment(0, 0, 1, 1));
            var e = new ElementList(e1);
            var res = e.CreateListSortedByLength();
            Assert.AreEqual(new Segment(0,0,0,0), res[0]);
            Assert.AreEqual(new Segment(0,0,1,1), res[1]);
            Assert.AreEqual(new Segment(0,0,2,2), res[2]);
            Assert.AreEqual(new Segment(0,0,10,10), res[3]);
        }
    }
}