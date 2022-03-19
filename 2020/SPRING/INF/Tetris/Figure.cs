using System;
using static Tetris.Constants;

namespace Tetris
{
    public class Figure
    {
        public readonly int Type;
        private int[,] coordinates;
        private readonly int[,] field;
        public bool Moving;
        private readonly int axis;
        
        public Figure(int type, int[,] field)
        {
            Moving=false; //Not sure if we need it
            this.field = field;
            Type = type;
            coordinates = (int[,])FigurePresets.FigurePresetsArray[type].Coordinates.Clone();
            axis = FigurePresets.FigurePresetsArray[type].Axis;
            for (var i = 0; i <= coordinates.GetLength(0) - 1; i++)
                field[coordinates[i, 0], coordinates[i, 1]] = type;
        }

        private int[,] PrepareTurn()
        {
            var newCoordinates = new int[coordinates.GetLength(0), 2];
            if (axis == -1)
                return coordinates;
            var axisX = coordinates[axis, 0];
            var axisY = coordinates[axis, 1];
            for (var i = 0; i < coordinates.GetLength(0); i++)
            {
                newCoordinates[i, 0] = -(coordinates[i, 1] - axisY) + axisX;
                newCoordinates[i, 1] = coordinates[i, 0] - axisX + axisY;
            }
            return newCoordinates;
        }

        private bool CanTurn(int[,] newCoordinates)
        {
            for (var i = 0; i <= newCoordinates.GetLength(0) - 1; i++)
            {
                if (newCoordinates[i, 0] > FieldWidth - 1 || newCoordinates[i, 1] > FieldHeight - 1)
                {
                    return false;
                }
                if (newCoordinates[i, 0] < 0 || newCoordinates[i, 1] < 0)
                {
                    return false;
                }
                var test = true;
                for (var i2 = 0; i2 <= newCoordinates.GetLength(0) - 1; i2 += 1)
                {

                    if (coordinates[i2, 1] == newCoordinates[i, 1] && coordinates[i2, 0] == newCoordinates[i, 0])
                        test = false;
                }
                if (field[newCoordinates[i, 0], newCoordinates[i, 1]] != 0 && test)
                    return false;
            }
            return true;
        }

        ///Turns the figure if possible
        public void Turn()
        {
            var newCoordinates = PrepareTurn();
            if (CanTurn(newCoordinates))
            {
                //if (turnType < 4)
                //    turnType = turnType + 1;
                //else
                //    turnType = 1;
                for (var n = 0; n <= coordinates.GetLength(0) - 1; n += 1)
                {
                    field[coordinates[n, 0], coordinates[n, 1]] = 0;
                    coordinates[n, 0] = newCoordinates[n, 0];
                    coordinates[n, 1] = newCoordinates[n, 1];
                }
                for (var i = 0; i <= coordinates.GetLength(0) - 1; i++)
                {
                    field[coordinates[i, 0], coordinates[i, 1]] = Type;
                }
            }
        }

        public bool HasSupport()
        {
            for (var l = 0; l <= coordinates.GetLength(0) - 1; l += 1)
            {
                if (coordinates[l, 1] == FieldHeight - 1)
                    return true;
                var test = true;
                for (var l2 = 0; l2 <= coordinates.GetLength(0) - 1; l2 += 1)
                    if (coordinates[l2, 1] == coordinates[l, 1] + 1 && coordinates[l2, 0] == coordinates[l, 0])
                        test = false;
                if (field[coordinates[l, 0], coordinates[l, 1] + 1] != 0 && test)
                    return true;
            }
            return false;
        }

        //-1 - left
        //0 - down
        //1 - right
        private bool CanMove(int direction)
        {
            if (direction == 0)
                return !HasSupport();
            if (direction < -1 || direction > 1)
                throw new ArgumentException();
            var a = 0;
            if (direction == -1)
                a = 0;
            if (direction == 1)
                a = FieldWidth - 1;
            for (var k = 0; k <= coordinates.GetLength(0) - 1; k += 1)
            {
                if (coordinates[k, 0] == a)
                    return false;
                var test = true;
                for (var k2 = 0; k2 <= coordinates.GetLength(0) - 1; k2 += 1)
                    if (coordinates[k2, 0] == coordinates[k, 0] + direction && coordinates[k2, 1] == coordinates[k, 1])
                        test = false;
                if (field[coordinates[k, 0] + direction, coordinates[k, 1]] != 0 && test)
                    return false;
            }
            return true;
        }

        public void Move(int direction)
        {
            if (CanMove(direction))
            {
                if (direction == 0)
                {
                    for (var p = 0; p <= coordinates.GetLength(0) - 1; p += 1)
                    {
                        field[coordinates[p, 0], coordinates[p, 1]] = 0;
                        coordinates[p, 1] += 1;
                    }
                    for (var p = 0; p <= coordinates.GetLength(0) - 1; p += 1)
                    {
                        field[coordinates[p, 0], coordinates[p, 1]] = Type;
                    }
                }
                else
                {
                    for (var p = 0; p <= coordinates.GetLength(0) - 1; p += 1)
                    {
                        field[coordinates[p, 0], coordinates[p, 1]] = 0;
                        coordinates[p, 0] += direction;
                    }
                    for (var p = 0; p <= coordinates.GetLength(0) - 1; p += 1)
                    {
                        field[coordinates[p, 0], coordinates[p, 1]] = Type;
                    }
                }
            }
        }
    }
}