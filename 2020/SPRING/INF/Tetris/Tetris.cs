using System;
using System.Threading;
using static Tetris.Constants;

namespace Tetris
{
    public class Tetris
    {
        private int[,] field;
        private int score;
        private bool isGameOver;

        public Figure Figure { get; private set; }

        private int timerInterval;

        public int TimerInterval => timerInterval;

        public bool IsGameOver => isGameOver;

        public int[,] Field => field;

        public int Score => score;

        public Tetris()
        {
            field = new int[FieldWidth, FieldHeight];
            score = 0;
            Figure = RandomFigure();
            timerInterval = MaxInterval;
        }

        public Tetris(int figureType)
        {
            field = new int[FieldWidth, FieldHeight];
            score = 0;
            Figure = new Figure(figureType, field);
            //timerInterval = MaxInterval;
        }

        private bool IsRowFull(int rowNumber) 
        {
            for (var i = 0; i <= FieldWidth - 1; i++) {
                if (field[i, rowNumber] == 0)
                    return false;
            }
            return true;
        }
        
        private void CheckIfGameOver() 
        {
            for (var i = 0; i <= FieldWidth - 1; i++)
                if (field[i, 0] != 0)
                {
                    isGameOver = true;
                    return;
                }
        }


        private Figure RandomFigure()
        {
            var random = new Random();
            var type = random.Next(FigureCount) + 1;
            var turn = random.Next(4) + 1;
            var figure = new Figure(type, field);
            for (var i = 0; i < turn; i++) //Necessary to randomize turn
                figure.Turn();
            return figure;
        }

        public void ClearFullRows()
        {
            for (var row = 0; row <= FieldHeight - 1; row += 1)
            {
                if (!IsRowFull(row)) continue;
                for (var x = 0; x <= FieldWidth - 1; x += 1)
                {
                    field[x, row] = 0;
                }
                score += 1;
                if (Math.Abs(score % (double)DecreaseIntervalAtScore) < 1e-6 && timerInterval > MinInterval)
                {
                    timerInterval -= IntervalStep;
                }
                for (var movingRow = row - 1; movingRow >= 0; movingRow -= 1)
                {
                    for (var cellX = 0; cellX <= FieldWidth - 1; cellX += 1)
                    {
                        field[cellX, movingRow + 1] = field[cellX, movingRow];
                        field[cellX, movingRow] = 0;
                    }
                }
            }
        }


        public void Tick()
        {
            if (Figure.HasSupport())
            {
                Figure = null;
                ClearFullRows();
                CheckIfGameOver();
                if (!isGameOver)
                    Figure = RandomFigure();
            }
            else
            {
                while (Figure.Moving)
                    Thread.Sleep(50);
                Figure.Moving = true;
                Figure.Move(0);
                Figure.Moving = false;
            }
        }
    }
}