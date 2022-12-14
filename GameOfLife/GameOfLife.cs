using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife {
    internal class GameOfLife {
        static private int countiter = 0;
        private readonly int height;
        private readonly int width;
        private Cell[,] matrix;
        Random rand = new Random();

        public GameOfLife(int height, int width, int frequency) {
            this.height = height;
            this.width = width;
            matrix = new Cell[height, width];
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    matrix[i, j] = new Cell(frequency);
                }
            }
        }

        public int Population(int y, int x) {
            int count = 0;
            for (int i = -1; i < 2; i++) {
                for (int j = -1; j < 2; j++) {
                    if (matrix[(y + i + height) % height, (x + j + width) % width].state && (i != 0 || j != 0)) count++;
                }
            }
            return count;
        }

        public void Iter() {
            int popul;
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    popul = Population(i, j);
                    matrix[i, j].nextGen = (popul == 3 || (popul == 2 && matrix[i, j].state));
                }
            }
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    matrix[i, j].Update();
                }
            }
            //matrix = updatematrix;
            countiter++;

        }

        public void Output() {
            char[] screen = new char[width];
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    screen[j] = (matrix[i, j].state) ? '#' : ' ';
                }
                Console.Write(screen);
            }
        }

        public int GetIter() {
            return countiter;
        }
        public bool[,] GetMatrix() {
            bool[,] result = new bool[width, height];
            for (int i = 0; i < height; i++) {
                for (int j = 0; j < width; j++) {
                    result[i, j] = matrix[i, j].state;
                }
            }
            return result;
        }
    }
}
