namespace Tetris
{
    public static class Constants
    {
        #region Constants related to field size
        public const int FieldWidth = 10;
        public const int FieldHeight = 15;
        public const int Middle = FieldWidth / 2;
        #endregion
        
        public const int FigureCount = 7;

        #region Constants related to window
        ///Width of the window is multiplied on this value to create a space to the right for scores, menu...
        public const double WidthMultiplier = 1.2;
        public const int VerticalOffset = 10;
        public const int StartHeight = 600;
        #endregion
        
        #region Constants related to font
        public const int DefaultFontSize = 25;
        public const string AppFont = "Arial";
        #endregion

        #region Constants related to timer
        ///Minimal possible timer interval
        public const int MinInterval = 200;
        ///Starting timer interval
        public const int MaxInterval = 1000;
        ///Value on which the interval decreases each step
        public const int IntervalStep = 100;
        ///After how many removed rows the interval decreases
        public const int DecreaseIntervalAtScore = 5;

        #endregion

        #region Strings
        public const string AppName = "Тетрис";
        public const string GameOverText = "Игра окончена";
        public const string StartGameText = "Нажмите 'P', чтобы начать игру\nH - справка";
        public const string PauseText = "Пауза. Нажмите 'P', чтобы возобновить игру";
        public const string HelpText = "A - передвинуть фигуру направо\nS - передвинуть фигуру налево\nS - передвинуть фигуру вниз\nW - повернуть фигуру\nP - пауза\nR - рекорды\n< и > - изменить тему";
        public const string HelpBoxTitle = "Справка";
        public const string RecordsBoxTitle = "Рекорды";
        #endregion

        public const int RecordNumber = 10;
        public const string RecordsFilename = "Records.tetris";
        public const string SettingsFilename = "Settings.tetris";
    }
}