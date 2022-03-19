using System;
using System.Collections.Generic;
using System.Text;

namespace SeminarTasks
{
    enum ChessPieces
    {
        Bishop,
        Knight,
        Rook,
        Queen,
        King
    }
    class Branches1
    {
        public static bool IsMoveCorrect(ChessPieces piece, int x0, int y0, int x, int y)
        {
            if ((x0 > 8) || (y0 > 8) || (x > 8) || (y > 8))
                return false;
            if ((Math.Abs(x - x0) == 0) && (Math.Abs(y - y0) == 0))
                return false;
            switch (piece)
            {
                case ChessPieces.Bishop:
                    if (Math.Abs(x - x0) == Math.Abs(y - y0))
                        return true;
                    break;
                case ChessPieces.Rook:
                    if ((x - x0 == 0) || (y - y0 == 0))
                        return true;
                    break;
                case ChessPieces.Queen:
                    if ((x - x0 == 0) || (y - y0 == 0) || (Math.Abs(x - x0) == Math.Abs(y - y0)))
                        return true;
                    break;
                case ChessPieces.King:
                    if ((Math.Abs(x - x0) <=1) && (Math.Abs(y - y0) <= 1))
                        return true;
                    break;
                case ChessPieces.Knight:
                    if ((Math.Abs(x - x0) == 1) && (Math.Abs(y - y0) == 2) || (Math.Abs(x - x0) == 2) && (Math.Abs(y - y0) == 1))
                        return true;
                    break;
            }
            return false;
        }
    }
}
