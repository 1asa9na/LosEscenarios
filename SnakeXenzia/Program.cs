using System;

namespace SnakeGame {
    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }

    public class Game {

        public const int WIDTH = 25;
        public const int HEIGHT = 20;
        public const int fontAspect = 2;
        public const int BlockSize = 2;
        static int screenWidth = WIDTH * fontAspect * BlockSize;
        static int screenHeight = HEIGHT * BlockSize;

        public static int DelayMs = 50;
        public static Random rand = new Random();

        public static void Main(string[] args) {

            Console.SetBufferSize(screenWidth, screenHeight + 20);
            Console.SetWindowSize(screenWidth, screenHeight);
            Console.CursorVisible = false;

            int highscore = 0;

            while (true) {

                Console.Clear();

                DrawBorders();

                Snake snake = new Snake(5, 5);

                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                Direction currDir = Direction.Right;

                Pixel apple = SetFood();

                int score = 0;
                Console.Title = $"SCORE: {score} SPEED: {1000.0 / DelayMs}";

                while (true) {

                    bool foodEaten = false;

                    sw.Restart();

                    Direction oldCurrDir = currDir;

                    if (snake.Head.coordX == apple.coordX && snake.Head.coordY == apple.coordY
                            || snake.Body.Any(c => c.coordY == apple.coordY && c.coordX == apple.coordX)) {
                        score += (int)(1000.0 / DelayMs);
                        Console.Title = $"SCORE: {score} SPEED: {1000.0 / DelayMs}";
                        apple = SetFood();
                        foodEaten = !foodEaten;
                    }

                    while (sw.ElapsedMilliseconds <= DelayMs) if (oldCurrDir == currDir) currDir = ReadMovementKey(currDir);

                    snake.Move(currDir, foodEaten);

                    if (snake.Head.coordX == 0
                        || snake.Head.coordX == WIDTH - 1
                        || snake.Head.coordY == 0
                        || snake.Head.coordY == HEIGHT - 1
                        || snake.Body.Any(a => a.coordX == snake.Head.coordX && a.coordY == snake.Head.coordY))
                        break;

                    Thread.Sleep(200);
                }
                highscore = (highscore > score) ? highscore : score;
                Console.SetCursorPosition(screenWidth / 3, screenHeight / 2);
                Console.Write("GAME OVER!");
                Console.SetCursorPosition(screenWidth / 3, screenHeight / 2 + 1);
                Console.Write($"YOUR SCORE:\t{score};");
                Console.SetCursorPosition(screenWidth / 3, screenHeight / 2 + 2);
                Console.Write($"HIGHSCORE:\t{highscore};");
                Console.SetCursorPosition(screenWidth / 3, screenHeight / 2 + 4);
                Console.Write("PRESS ANY BUTTON TO CONTINUE");
                Console.ReadKey();
            }
        }
        public static Direction ReadMovementKey(Direction currDir) {
            if (!Console.KeyAvailable) return currDir;
            else {
                ConsoleKey key = Console.ReadKey(true).Key;

                currDir = key switch {
                    ConsoleKey.W => (currDir != Direction.Down) ? Direction.Up : currDir,
                    ConsoleKey.A => (currDir != Direction.Right) ? Direction.Left : currDir,
                    ConsoleKey.S => (currDir != Direction.Up) ? Direction.Down : currDir,
                    ConsoleKey.D => (currDir != Direction.Left) ? Direction.Right : currDir,
                    _ => currDir
                };
                return currDir;
            }
        }
        public static void DrawBorders() {
            for (int i = 0; i < WIDTH; i++) {
                new Pixel(i, 0, ConsoleColor.White).Draw();
                new Pixel(i, HEIGHT - 1, ConsoleColor.White).Draw();
            }
            for (int i = 0; i < HEIGHT; i++) {
                new Pixel(0, i, ConsoleColor.White).Draw();
                new Pixel(WIDTH - 1, i, ConsoleColor.White).Draw();
            }
        }

        public static Pixel SetFood() {
            int x = rand.Next(1, WIDTH - 1);
            int y = rand.Next(1, HEIGHT - 1);
            Pixel food = new Pixel(x, y, ConsoleColor.Gray);
            food.Draw();
            return food;
        }
    }

}