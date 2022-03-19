using static Tetris.Constants;

namespace Tetris
{
    public class FigurePreset
    {
        public readonly int[,] Coordinates;
        public readonly int Axis;

        public FigurePreset(int[,] coordinates, int axis)
        {
            Coordinates = coordinates;
            Axis = axis;
        }
    }
    
    public static class FigurePresets
    {
        private static readonly FigurePreset Placeholder = new FigurePreset(null, -1);
        private static readonly FigurePreset F1 = new FigurePreset(new[,]{{Middle - 2, 0}, {Middle - 1, 0}, {Middle, 0}, {Middle + 1, 0}}, 1);//Stick
        private static readonly FigurePreset F2 = new FigurePreset(new[,]{{Middle - 1, 0}, {Middle, 0}, {Middle - 1, 1}, {Middle, 1}}, -1);//Cube
        private static readonly FigurePreset F3 = new FigurePreset(new[,]{{Middle - 1, 0}, {Middle, 0}, {Middle, 1}, {Middle + 1, 1}}, 1);
        private static readonly FigurePreset F4 = new FigurePreset(new[,]{{Middle, 0}, {Middle + 1, 0}, {Middle - 1, 1}, {Middle, 1}}, 0);
        private static readonly FigurePreset F5 = new FigurePreset(new[,]{{Middle - 1, 1}, {Middle, 1}, {Middle + 1, 1}, {Middle - 1, 0}}, 1);//Г
        private static readonly FigurePreset F6 = new FigurePreset(new[,]{{Middle - 1, 1}, {Middle, 1}, {Middle + 1, 1}, {Middle + 1, 0}}, 1);//Г
        private static readonly FigurePreset F7 = new FigurePreset(new[,]{{Middle - 1, 1}, {Middle, 1}, {Middle + 1, 1}, {Middle, 0}}, 1);//T

        public static readonly FigurePreset[] FigurePresetsArray = {Placeholder, F1, F2, F3, F4, F5, F6, F7};
    }
}