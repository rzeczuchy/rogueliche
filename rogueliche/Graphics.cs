using System;

namespace rogueliche
{
    public class Graphics
    {
        private readonly char[][] buffer;
        
        private const int DefaultWindowWidth = 102;
        private const int DefaultWindowHeight = 68;

        public Graphics()
        {
            ResetBufferAndWindowSize();

            Width = Console.BufferWidth;
            Height = Console.BufferHeight - 1;

            buffer = new char[Height][];
            for (int y = 0; y < Height; y++)
            {
                buffer[y] = new char[Width];
            }
            Clear();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

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

        private void ResetBufferAndWindowSize()
        {
            if (Console.WindowWidth != Console.BufferWidth || Console.WindowHeight != Console.BufferHeight)
            {
                Console.SetWindowSize(1, 1);
                int windowWidth = Console.LargestWindowWidth > DefaultWindowWidth ? DefaultWindowWidth : Console.LargestWindowWidth - 1;
                int windowHeight = Console.LargestWindowHeight > DefaultWindowHeight ? DefaultWindowHeight : Console.LargestWindowHeight - 1;
                Console.SetBufferSize(windowWidth, windowHeight);
                Console.SetWindowSize(windowWidth, windowHeight);
            }
        }

        public void Draw()
        {
            ResetBufferAndWindowSize();
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
    }
}
