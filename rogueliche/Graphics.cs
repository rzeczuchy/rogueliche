using System;
using System.Text;

namespace rogueliche
{
    public class Graphics
    {
        private readonly char[][] buffer;
        
        public const int BufferWidth = 80;
        public const int BufferHeight = 50;

        public Graphics()
        {
            ConfigureGraphics();
            ResetBufferAndWindow();

            buffer = new char[Height][];
            for (int y = 0; y < Height; y++)
            {
                buffer[y] = new char[Width];
            }
            Clear();
        }

        public int Width { get => BufferWidth; }
        public int Height { get => BufferHeight; }

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                {
                    buffer[y][x] = ' ';
                }
        }

        public void DrawChar(char c, int x, int y)
        {
            if (IsWithinBuffer(x, y))
            {
                buffer[y][x] = c;
            }
        }

        public void DrawChar(char c, Point pos)
        {
            DrawChar(c, pos.X, pos.Y);
        }

        public void DrawString(string str, int x, int y)
        {
            for (int c = 0; c < str.Length; c++)
            {
                char toPrint = str[c];
                if (IsWithinBuffer(x + c, y))
                {
                    buffer[y][x + c] = toPrint;
                }
            }
        }

        public void DrawString(string str, Point pos)
        {
            DrawString(str, pos.X, pos.Y);
        }

        public void DrawCenteredString(string str, int y)
        {
            DrawString(str, Width / 2 - str.Length / 2, y);
        }

        public void DrawBar(double current, double max, double length, int x, int y)
        {
            if (max > 0 && length > 0)
            {
                double percentToColor = current / max;

                for (int c = 0; c < length; c++)
                {
                    double currentPercent = c / length;

                    if (IsWithinBuffer(x + c, y) && currentPercent >= percentToColor)
                        buffer[y][x + c] = '\u2591';
                    else if (IsWithinBuffer(x + c, y) && currentPercent < percentToColor)
                        buffer[y][x + c] = '\u2588';
                }
            }
        }

        public void Draw()
        {
            ResetBufferAndWindow();
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < Height; y++)
            {
                Console.Write(buffer[y]);
            }
            Clear();
        }

        public bool IsWithinBuffer(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool IsWithinBuffer(Point pos)
        {
            return IsWithinBuffer(pos.X, pos.Y);
        }

        public static Point CameraTransform(Player player, Graphics render)
        {
            return new Point(player.Position.X - render.Width / 2, player.Position.Y - render.Height / 2);
        }

        private static void ConfigureGraphics()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            Console.OutputEncoding = Encoding.Unicode;
        }

        private void ResetBufferAndWindow()
        {
            if (BufferAndWindowNeedResetting())
            {
                Console.SetWindowPosition(0, 0);
                Console.SetWindowSize(1, 1);
                Console.SetBufferSize(AdjustedWindowWidth(), AdjustedWindowHeight());
                Console.SetWindowSize(AdjustedWindowWidth(), AdjustedWindowHeight());
            }
        }

        private bool BufferAndWindowNeedResetting()
        {
            return Console.BufferWidth != AdjustedWindowWidth()
                || Console.BufferHeight != AdjustedWindowHeight()
                || Console.WindowWidth != AdjustedWindowWidth()
                || Console.WindowHeight != AdjustedWindowHeight()
                || Console.WindowLeft != 0
                || Console.WindowTop != 0;
        }

        private int AdjustedWindowWidth()
        {
            return BufferWidth < Console.LargestWindowWidth ? BufferWidth : Console.LargestWindowWidth;
        }

        private int AdjustedWindowHeight()
        {
            return BufferHeight + 1 < Console.LargestWindowHeight ? BufferHeight + 1 : Console.LargestWindowHeight;
        }
    }
}
