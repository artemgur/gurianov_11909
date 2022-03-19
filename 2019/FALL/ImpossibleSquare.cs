using System;
using System.Diagnostics;
using System.Drawing;

namespace RefactorMe
{
    // ## Прочитайте! ##
    //
    // Ваша задача привести код в этом файле в порядок. 
    // Для начала запустите эту программу. Для этого в VS в проект подключите сборку System.Drawing.

    // Переименуйте всё, что называется неправильно. Это можно делать комбинацией клавиш Ctrl+R, Ctrl+R (дважды нажать Ctrl+R).
    // Повторяющиеся части кода вынесите во вспомогательные методы. Это можно сделать выделив несколько строк кода и нажав Ctrl+R, Ctrl+M
    // Избавьтесь от всех зашитых в коде числовых констант — положите их в переменные с понятными именами.
    // 
    // После наведения порядка проверьте, что ваш код стал лучше. 
    // На сколько проще после ваших переделок стало изменить размер фигуры? Повернуть её на некоторый угол? 
    // Научиться рисовать невозможный треугольник, вместо квадрата?

    class Drawer
    {
        const int imageWidth = 800;
        const int imageHeight = 600;
        static Bitmap image = new Bitmap(imageWidth, imageHeight);
        static float x, y;
        static Graphics graphics;

        public static void Initialize()
        {
            image = new Bitmap(imageWidth, imageHeight);
            graphics = Graphics.FromImage(image);
        }

        public static void SetPos(float x0, float y0)
        {
            x = x0;
            y = y0;
        }

        public static void DrawLine(double L, double angle)
        {
            //Делает шаг длиной L в направлении angle и рисует пройденную траекторию
            var x1 = (float)(x + L * Math.Cos(angle));
            var y1 = (float)(y + L * Math.Sin(angle));
            graphics.DrawLine(Pens.Yellow, x, y, x1, y1);
            x = x1;
            y = y1;
        }

        public static void ShowResult()
        {
            image.Save("result.bmp");
            Process.Start("result.bmp");
        }
    }

    public class ImpossibleSquare
    {
        const float squareSize = 100;
        const float angleSize = 10;

        public static void Main()
        {
            Drawer.Initialize();
            DrawPart(angleSize, 0, 0);
            DrawPart(2*angleSize+squareSize, angleSize, Math.PI / 2);
            DrawPart(angleSize + squareSize, 2 * angleSize + squareSize, Math.PI);
            DrawPart(0, angleSize + squareSize, -Math.PI / 2);
            Drawer.ShowResult();
        }
        public static void DrawPart(float startX,float startY,double angle)
        {
            Drawer.SetPos(startX, startY);
            Drawer.DrawLine(squareSize, angle);
            Drawer.DrawLine(angleSize * Math.Sqrt(2), angle + Math.PI / 4);
            Drawer.DrawLine(squareSize, angle + Math.PI);
            Drawer.DrawLine(squareSize - angleSize, angle + Math.PI / 2);
        }

    }
}