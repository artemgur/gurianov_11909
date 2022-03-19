using System;
using System.Linq;
using NUnit.Framework;
using static Tetris.Constants;

namespace Tetris
{
    public static class TetrisTests
    {
        [Test]
        public static void MoveDownTest()
        {
            var tetris = new Tetris(1);
            tetris.Figure.Move(0);
            var row = Enumerable.Range(0, FieldWidth).Select(x => tetris.Field[x, 1]).ToArray();
            Assert.AreEqual(new[]{0, 0, 0, 1, 1, 1, 1, 0, 0, 0}, row);
        }
        
        [Test]
        public static void MoveLeftTest()
        {
            var tetris = new Tetris(1);
            tetris.Figure.Move(-1);
            var row = Enumerable.Range(0, FieldWidth).Select(x => tetris.Field[x, 0]).ToArray();
            Assert.AreEqual(new[]{0, 0, 1, 1, 1, 1, 0, 0, 0, 0}, row);
        }

        [Test]
        public static void MoveRightTest()
        {
            var tetris = new Tetris(1);
            tetris.Figure.Move(1);
            var row = Enumerable.Range(0, FieldWidth).Select(x => tetris.Field[x, 0]).ToArray();
            Assert.AreEqual(new[]{0, 0, 0, 0, 1, 1, 1, 1, 0, 0}, row);
        }
        
        [Test]
        public static void ImpossibleRotationTest()
        {
            var tetris = new Tetris(1);
            tetris.Figure.Turn();
            var row = Enumerable.Range(0, FieldWidth).Select(x => tetris.Field[x, 0]).ToArray();
            Assert.AreEqual(new[]{0, 0, 0, 1, 1, 1, 1, 0, 0, 0}, row);
        }
        
        [Test]
        public static void PossibleRotationTest()
        {
            var tetris = new Tetris(1);
            tetris.Figure.Move(0);
            tetris.Figure.Turn();
            var row = Enumerable.Range(0, FieldHeight).Select(x => tetris.Field[4, x]).ToArray();
            Assert.AreEqual(new[]{1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}, row);
        }

        [Test]
        public static void ClearRowTest()
        {
            var tetris = new Tetris(1);
            for (var i = 0; i < 3; i++)
            {
                tetris.Field[i, FieldHeight - 1] = 2;
                tetris.Field[FieldWidth - 1 - i, FieldHeight - 1] = 2;
            }
            for (var i = 0; i < FieldHeight - 1; i++)
                tetris.Figure.Move(0);
            tetris.ClearFullRows();
            var row = Enumerable.Range(0, FieldWidth).Select(x => tetris.Field[x, FieldHeight - 1]).ToArray();
            Assert.AreEqual(Enumerable.Repeat(0, FieldWidth), row);
            Assert.AreEqual(1, tetris.Score);
        }

        [Test]
        public static void ClearSeveralRowsTest()
        {
            var tetris = new Tetris(1);
            for (var i = 0; i < FieldWidth; i++)
                if (i != 4)
                {
                    tetris.Field[i, FieldHeight - 1] = 2;
                    tetris.Field[i, FieldHeight - 2] = 2;
                }
            tetris.Figure.Move(0);
            tetris.Figure.Turn();
            for (var i = 0; i < FieldHeight - 4; i++)
                tetris.Figure.Move(0);
            tetris.ClearFullRows();
            var row1 = Enumerable.Range(0, FieldWidth).Select(x => tetris.Field[x, FieldHeight - 1]).ToArray();
            var row2 = Enumerable.Range(0, FieldWidth).Select(x => tetris.Field[x, FieldHeight - 2]).ToArray();
            var expected = Enumerable.Repeat(0, FieldWidth).ToArray();
            expected[4] = 1;
            Assert.AreEqual(expected, row1);
            Assert.AreEqual(expected, row2);
            Assert.AreEqual(2, tetris.Score);
        }

        [Test]
        public static void AddRecordTest()
        {
            var records = RecordManager.ReadRecords().Split(new[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            RecordManager.WriteRecords(Enumerable.Range(1, 10));
            RecordManager.AddResult(8);
            Assert.AreEqual("10\r\n9\r\n8\r\n8\r\n7\r\n6\r\n5\r\n4\r\n3\r\n2\r\n", RecordManager.ReadRecords());
            RecordManager.WriteRecords(records);
        }
    }
}