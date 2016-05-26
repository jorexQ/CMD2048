using CMD2048.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMD2048.Handle
{
    public static class Game
    {
        public static void Init(int x, int y)
        {
            var gameWindwo = InitWindow(x, y);
            var core = new Core(x, y);
            var image = core.GetImage();
            gameWindwo.Refurbish(image);
            Console.Read();
        }

        public static GameWindow InitWindow(int x,int y)
        {
            return new GameWindow(x, y)
                .RegisterCellStyle(2, ConsoleColor.Black, ConsoleColor.White)
                .RegisterCellStyle(4, ConsoleColor.Black, ConsoleColor.Gray)
                .RegisterCellStyle(8, ConsoleColor.Black, ConsoleColor.DarkGray)
                .RegisterCellStyle(16, ConsoleColor.Black, ConsoleColor.DarkYellow)
                .RegisterCellStyle(32, ConsoleColor.Black, ConsoleColor.Yellow)
                .RegisterCellStyle(64, ConsoleColor.Black, ConsoleColor.Green)
                .RegisterCellStyle(128, ConsoleColor.Black, ConsoleColor.DarkGreen)
                .RegisterCellStyle(256, ConsoleColor.Black, ConsoleColor.DarkCyan)
                .RegisterCellStyle(512, ConsoleColor.Black, ConsoleColor.Cyan)
                .RegisterCellStyle(1024, ConsoleColor.Black, ConsoleColor.Blue)
                .RegisterCellStyle(2048, ConsoleColor.Black, ConsoleColor.DarkBlue)
                .RegisterCellStyle(4096, ConsoleColor.Black, ConsoleColor.DarkMagenta)
                .RegisterCellStyle(8192, ConsoleColor.Black, ConsoleColor.Magenta)
                .RegisterCellStyle(16384, ConsoleColor.Black, ConsoleColor.Red)
                .RegisterCellStyle(32768, ConsoleColor.Black, ConsoleColor.DarkRed);
        }
    }
}
