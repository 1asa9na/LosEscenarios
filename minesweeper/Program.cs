using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper {
    class Program {
        static void Main(string[] args) {
            Console.CursorVisible = false;
            Console.WindowLeft = 0;
            Console.WindowTop = 0;
            Console.WriteLine("Введите высоту: ");
            int height = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите ширину: ");
            int width = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите частоту: ");
            int freq = Convert.ToInt32(Console.ReadLine());
            Console.SetBufferSize(width * 3, height);
            Console.SetWindowSize(width * 3, height);
            GameEngine Start = new GameEngine(height, width, freq);
            Console.Clear();
            Start.Step();
        }
    }
    class GameEngine {
        private bool FirstStep = true;
        private int y = 0, x = 0;
        private bool stop = false;
        private ConsoleKeyInfo key;
        private readonly int height;
        private readonly int width;
        private int[,] bombs;
        private int[,] stepfield;
        private readonly int freq;
        private Random rnd = new Random();

        public GameEngine(int h, int w, int a) {
            height = h;
            width = w;
            freq = a;
            stepfield = new int[height,width];
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    stepfield[i, j] = -1;
                }
            }
            bombs = new int[height,width];
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    bombs[i, j] = -1;
                }
            }
        }

        public void NewField() {
            FirstStep = false;
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    bombs[i, j] = 2*Convert.ToInt32(i != y && j != y && rnd.Next() % freq == 0)-1;
                }
            }
        }

        public void Wave(int a, int b) {
            int popul = Population(a, b);
            stepfield[a, b] = popul;
            bombs[a, b] = 0;
            if (popul == 0) {
                for (int i = -1; i < height; i++) {
                    for (int j = -1; j < width; j++) {
                        if (InBorders(a+i, j+b) && stepfield[a+i, b+j]==-1 && bombs[a+i, b+j] == -1 && (i != 0 || j != 0)) Wave((i + a + height) % height, (j + b + width) % width);
                    }
                }
            }
        }

        public int Population(int a, int b) {
            int count = 0;
            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++) {
                    count += (InBorders(i+a, j+b) && bombs[i + a, j + b]==1 && (i != 0 || j != 0)) ? 1 : 0;
                }
            }
            return count;
        /*
         *  -1 : НЕИЗВЕСТНАЯ КЛЕТКА
         * 0-8 : ИЗВЕСТНАЯ КЛЕТКА
         *   9 : ФЛАГ
        */   
        }

        public bool InBorders(int a, int b) {
            return (a >= 0 && a < height && b >= 0 && b < width);
        }

        public void Step() {
            Output();
            while (!stop) {
                key = Console.ReadKey();
                switch (key.Key) {
                    case ConsoleKey.UpArrow:
                        y += height;
                        y--;
                        y %= height;
                        break;
                    case ConsoleKey.DownArrow:
                        y++;
                        y %= height;
                        break;
                    case ConsoleKey.RightArrow:
                        x++;
                        x %= width;
                        break;
                    case ConsoleKey.LeftArrow:
                        x += width;
                        x--;
                        x %= width;
                        break;
                    case ConsoleKey.Escape:
                        stop = true;
                        break;
                    case ConsoleKey.Enter:
                        if (FirstStep) NewField();
                        if(bombs[y,x] == 1) stop = true;
                        Wave(y, x);
                        break;
                    case ConsoleKey.Tab:
                        if (stepfield[y, x] == 9) stepfield[y, x] = -1;
                        if (stepfield[y, x] == -1) stepfield[y, x] = 9;
                        break;
                }
                Output();
            }
            Console.Clear();
            if (bombs[y, x]==1) Console.WriteLine("Игра Окончена!");
        }

        public void Output() {
            for (int i = 0; i < height; i++) {
                string screen = "";
                for (int j = 0; j < width; j++) {
                    screen += (i==y && j==x) ? "\u001b[33m" : (stepfield[i, j] == -1) ? "\u001b[0m" : (stepfield[i, j] == 0) ? "\u001b[30m" : (stepfield[i, j] == 9) ? "\u001b[32m" : "\u001b[34m";
                    screen += '[';
                    screen += (stepfield[i, j]<0) ? '#' : (stepfield[i, j]==9) ? '!' : Convert.ToString(stepfield[i, j]);
                    screen += ']';
                }
                Console.Write(screen);
            }
            Console.Write("\u001b[0m");
        }
    }
}