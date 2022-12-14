using System;

namespace SnakeGame {
    public class Pixel {
        public readonly int coordX;
        public readonly int coordY;
        public readonly ConsoleColor color;
        public Pixel(int x, int y, ConsoleColor c) {
            coordX = x;
            coordY = y;
            color = c;
        }

        public void Draw() {
            for (int i = 0; i < Game.BlockSize; i++) {
                for (int j = 0; j < Game.BlockSize * Game.fontAspect; j++) {
                    Console.SetCursorPosition(coordX * Game.fontAspect * Game.BlockSize + j, coordY * Game.BlockSize + i);
                    Console.ForegroundColor = color;
                    Console.Write('█');
                }
            }
        }

        public void Clear() {
            for (int i = 0; i < Game.BlockSize; i++) {
                for (int j = 0; j < Game.BlockSize * Game.fontAspect; j++) {
                    Console.SetCursorPosition(coordX * Game.fontAspect * Game.BlockSize + j, coordY * Game.BlockSize + i);
                    Console.Write(' ');
                }
            }
        }
    }
}
