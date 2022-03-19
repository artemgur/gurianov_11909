﻿using System.Drawing;
 using System.IO;
 using System.Linq;
 using System.Windows.Forms;
using static Tetris.Constants;
using static Tetris.ThemePresets;

namespace Tetris
{
    public partial class GameForm : Form
    {
        private Tetris tetris;
        private Timer timer;
        private int themeIndex;
        private Theme theme;
        private bool timerPaused;
        private bool gameStarted;
        private Label score;
        private Label info;

        public GameForm()
        {
            DoubleBuffered = true;
            gameStarted = false;
            timerPaused = false;
            InitializeComponent();
            SetInitialSize();
            Text = AppName;
            Paint += TetrisPaint;
            Resize += (sender, args) => TetrisResize();
            ResizeEnd += (sender, args) => TetrisResizeEnd();
            KeyDown += TetrisKeyDown;
            Closing += (sender, args) => File.WriteAllText(SettingsFilename, themeIndex.ToString());
            var font = new Font(AppFont, DefaultFontSize);

            if (!File.Exists(SettingsFilename))
                File.WriteAllText(SettingsFilename, "0");
            themeIndex = int.Parse(File.ReadLines(SettingsFilename).Single());
            theme = Themes[themeIndex];
            
            score = new Label();
            score.BackColor = Color.Transparent;
            score.Font = font;
            score.ForeColor = theme.ScoreColor;
            score.TextAlign = ContentAlignment.TopRight;
            Controls.Add(score);
            
            info = new Label();
            info.BackColor = Color.Transparent;
            info.Dock = DockStyle.Fill;
            info.Font = font;
            info.ForeColor = theme.GameOverColor;
            info.TextAlign = ContentAlignment.MiddleCenter;
            Controls.Add(info);

            timer = new Timer();
            tetris = new Tetris();
            timer.Interval = MaxInterval;
            timer.Tick += (sender, args) => tetris.Tick();
            timer.Tick += (sender, args) => Tick();
        }

        private void UpdateGUI(float cellSize)
        {
            score.Top = 0;
            score.Left = (int) (cellSize * FieldWidth) + 2;
            score.Width = ClientSize.Width - score.Left;
            score.Height = score.Width;
            score.Text = tetris.Score.ToString();
            score.ForeColor = theme.ScoreColor;
            info.ForeColor = theme.GameOverColor;
            var scale = Height / (float)StartHeight;
            var font = new Font(AppFont, DefaultFontSize * scale);
            score.Font = font;
            info.Font = font;
            if (!gameStarted)
                info.Text = StartGameText;
            else if (tetris.IsGameOver)
                info.Text = GameOverText;
        }

        private void TetrisKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.P:
                    if (!gameStarted)
                        StartGame();
                    else
                        Pause();
                    break;
                case Keys.Left:
                case Keys.A:
                    FigureMove(-1);
                    break;
                case Keys.Down:
                case Keys.S:
                    FigureMove(0);
                    break;
                case Keys.Right:
                case Keys.D:
                    FigureMove(1);
                    break;
                case Keys.Up:
                case Keys.W:
                    FigureTurn();
                    break;
                case Keys.H:
                    ShowHelp();
                    break;
                case Keys.R:
                    ShowRecords();
                    break;
                case Keys.Oemcomma:
                    ChangeTheme(-1);
                    break;
                case Keys.OemPeriod:
                    ChangeTheme(1);
                    break;
            }
        }

        private void ChangeTheme(int x)
        {
            if (themeIndex == 0 && x == -1)
                themeIndex = Themes.Length - 1;
            else if (themeIndex == Themes.Length - 1 && x == 1)
                themeIndex = 0;
            else
                themeIndex += x;
            theme = Themes[themeIndex];
            Invalidate();
        }

        private void ShowRecords()
        {
            if (!timerPaused)
                Pause();
            MessageBox.Show(this, RecordManager.ReadRecords(), RecordsBoxTitle, MessageBoxButtons.OK);
        }

        private void ShowHelp()
        {
            if (!timerPaused)
                Pause();
            MessageBox.Show(this, HelpText, HelpBoxTitle, MessageBoxButtons.OK);
        }

        private void FigureTurn()
        {
            if (!timerPaused && tetris.Figure != null && !tetris.Figure.Moving)
            {
                tetris.Figure.Moving = true;
                tetris.Figure.Turn();
                tetris.Figure.Moving = false;
                Invalidate();
            }
        }

        private void FigureMove(int i)
        {
            if (!timerPaused && tetris.Figure != null && !tetris.Figure.Moving)
            {
                tetris.Figure.Moving = true;
                tetris.Figure.Move(i);
                tetris.Figure.Moving = false;
                Invalidate();
            }
        }

        private void Pause()
        {
            if (timerPaused)
            {
                if (!tetris.IsGameOver)
                    info.Text = "";
                timer.Start();
            }
            else
            {
                if (!tetris.IsGameOver)
                    info.Text = PauseText;
                timer.Stop();
            }
            timerPaused = !timerPaused;
        }

        private void StartGame()
        {
            gameStarted = true;
            info.Text = "";
            timer.Start();
        }

        private void TetrisResize()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                //Will work on all but extremely exotic monitors, where ClientSize.Height is more than ClientSize.Width
                var cellSize = ClientSize.Height / FieldHeight;
                WindowState = FormWindowState.Normal;
                //TODO header size
                ClientSize = new Size((int) (cellSize * FieldWidth * WidthMultiplier), cellSize * FieldHeight - VerticalOffset);
                Top = 0;
                var screenWidth = Screen.FromControl(this).WorkingArea.Width;
                Left = (screenWidth - ClientSize.Width) / 2;
            }
            if (WindowState == FormWindowState.Minimized)
            {
                timerPaused = true;
                timer.Stop();
            }
            Invalidate();
        }

        private void SetInitialSize()
        {
            Height = StartHeight;
            var cellSize = ClientSize.Height / FieldHeight;
            ClientSize = new Size((int) (cellSize * FieldWidth * WidthMultiplier), cellSize * FieldHeight - VerticalOffset);
        }

        private void TetrisResizeEnd()
        {
            var xCellSize = (int) (ClientSize.Width / WidthMultiplier / FieldWidth);
            var yCellSize = ClientSize.Height / FieldHeight;
            if (xCellSize < yCellSize)
                ClientSize = new Size(ClientSize.Width, xCellSize * FieldHeight);
            else
                ClientSize = new Size((int) (yCellSize * FieldWidth * WidthMultiplier), ClientSize.Height);
            Invalidate();
        }

        private void TetrisPaint(object sender, PaintEventArgs args)
        {
            var g = args.Graphics;
            var cellSize =
                (float) ClientSize.Height / FieldHeight; //Should be equal for ClientSize.Width and ClientSize.Height

            #region Drawing of cells

            var brush = new SolidBrush(theme.BackgroundColor);
            g.FillRectangle(brush, 0, 0, ClientSize.Width, ClientSize.Height);
            for (var m = 0; m <= FieldWidth - 1; m += 1)
            {
                for (var n = 0; n <= FieldHeight - 1; n += 1)
                {
                    brush.Color = theme.FigureColors[tetris.Field[m, n]];
                    if (brush.Color != theme.FigureColors[0])
                        g.FillRectangle(brush, cellSize * m, cellSize * n, cellSize, cellSize);
                }
            }

            #endregion

            #region Drawing of grid

            var pen = new Pen(theme.GridColor);
            for (var i = 1; i <= FieldWidth - 1; i++)
            {
                g.DrawLine(pen, i * cellSize, 0, i * cellSize, FieldHeight * cellSize);
            }
            for (var i = 1; i <= FieldHeight - 1; i++)
            {
                g.DrawLine(pen, 0, i * cellSize, FieldWidth * cellSize, i * cellSize);
            }
            pen = new Pen(theme.BorderColor);
            g.DrawLine(pen, 0, 0, 0, FieldHeight * cellSize);
            g.DrawLine(pen, FieldWidth * cellSize, 0, FieldWidth * cellSize, FieldHeight * cellSize);
            g.DrawLine(pen, 0, 0 * cellSize, FieldWidth * cellSize, 0 * cellSize);
            g.DrawLine(pen, 0, FieldHeight * cellSize, FieldWidth * cellSize, FieldHeight * cellSize);
            #endregion

            UpdateGUI(cellSize);
        }

        private void Tick()
        {
            Invalidate();
            if (tetris.IsGameOver)
            {
                timer.Stop();
                RecordManager.AddResult(tetris.Score);
            }
            if (timer.Interval > tetris.TimerInterval)
            {
                timer.Stop();
                timer.Interval = tetris.TimerInterval;
                timer.Start();
            }
        }
    }
}