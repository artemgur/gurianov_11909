using System.Drawing;

namespace Tetris
{
    public class Theme
    {
        public readonly Color GridColor;
        public readonly Color BorderColor;
        public readonly Color GameOverColor;
        public readonly Color ScoreColor;
        public readonly Color BackgroundColor;

        public readonly Color[] FigureColors;

        public Theme(Color gridColor, Color borderColor, Color backgroundColor, Color gameOverColor, Color scoreColor, Color[] figureColors)
        {
            GridColor = gridColor;
            BorderColor = borderColor;
            BackgroundColor = backgroundColor;
            GameOverColor = gameOverColor;
            ScoreColor = scoreColor;
            FigureColors = figureColors;
        }
    }

    public static class FigureColorsPresets
    {
        public static readonly Color[] Light = {Color.White, Color.Red, Color.Orange, Color.Green, Color.Blue, Color.DarkGreen, Color.DarkBlue, Color.DarkRed };

        public static readonly Color[] Dark =
        {
            Color.Black, Color.DarkRed, Color.DarkOrange, Color.DarkGreen, Color.DarkBlue,
            Color.FromArgb(255, 0, 100, 0), Color.FromArgb(255, 0, 0, 100), 
            Color.FromArgb(255, 100, 0, 0)
        };
    }

    public static class ThemePresets
    {
        public static readonly Theme Light = new Theme(Color.White, Color.Black, Color.White,  Color.Blue, Color.Green, FigureColorsPresets.Light);
        public static readonly Theme LightWithGrid = new Theme(Color.Black, Color.Black, Color.White,  Color.Blue, Color.Green, FigureColorsPresets.Light);
        
        public static readonly Theme Light2 = new Theme(Color.White, Color.Black, Color.White,  Color.Blue, Color.Green, FigureColorsPresets.Dark);
        public static readonly Theme Light2WithGrid = new Theme(Color.Black, Color.Black, Color.White,  Color.Blue, Color.Green, FigureColorsPresets.Dark);
        
        public static readonly Theme Dark = new Theme(Color.Black, Color.White, Color.Black, Color.LightBlue, Color.LightGreen, FigureColorsPresets.Dark);
        public static readonly Theme DarkWithGrid = new Theme(Color.White, Color.White, Color.Black, Color.LightBlue, Color.LightGreen, FigureColorsPresets.Dark);
        
        public static readonly Theme Dark2 = new Theme(Color.Black, Color.White, Color.Black, Color.LightBlue, Color.LightGreen, FigureColorsPresets.Light);
        public static readonly Theme Dark2WithGrid = new Theme(Color.White, Color.White, Color.Black, Color.LightBlue, Color.LightGreen, FigureColorsPresets.Light);

        public static readonly Theme[] Themes =
            {Light, LightWithGrid, Light2, Light2WithGrid, Dark, DarkWithGrid, Dark2, Dark2WithGrid};

    }
}