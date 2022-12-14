using System;

namespace SnakeGame {
    class Snake {
        public Queue<Pixel> Body { get; } = new Queue<Pixel>();
        public Pixel Head { get; private set; }
        public Snake(int initialX, int initialY, int initialBodyLenght = 2) {
            Head = new Pixel(initialX, initialY, ConsoleColor.Green);
            for (int i = initialBodyLenght - 1; i >= 0; i--) {
                Body.Enqueue(new Pixel(initialX - i - 1, initialY, ConsoleColor.Yellow));
            }
        }

        public void Draw() {
            Head.Draw();
            foreach (Pixel pixel in Body) pixel.Draw();
        }

        public void Clear() {
            Head.Clear();
            foreach (Pixel pixel in Body) pixel.Clear();
        }

        public void Move(Direction dir, bool flag=false) {
            Body.Enqueue(new Pixel(Head.coordX, Head.coordY, ConsoleColor.Yellow));
            if(!flag) Body.Dequeue().Clear();
            Head = dir switch {
                Direction.Right => new Pixel(Head.coordX + 1, Head.coordY, ConsoleColor.Green),
                Direction.Left => new Pixel(Head.coordX - 1, Head.coordY, ConsoleColor.Green),
                Direction.Up => new Pixel(Head.coordX, Head.coordY - 1, ConsoleColor.Green),
                Direction.Down => new Pixel(Head.coordX, Head.coordY + 1, ConsoleColor.Green),
                _ => Head
            };
            Draw();
        }
    }
}