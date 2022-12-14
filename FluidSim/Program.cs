using System;

namespace FluidSim {
    public class Field {
        public const int ITER = 4;
        public const int SIZE = 100;
        public const int fontAspect = 2;
        public const int SCALE = 1;


        static Random rand = new Random();
        public static void Main() {
            Console.SetWindowSize(SIZE * fontAspect * SCALE, SIZE * SCALE);
            Console.SetBufferSize(SIZE * fontAspect * SCALE, SIZE * SCALE);

            Console.CursorVisible = false;


            FluidCube Fluid = new FluidCube(0, 0, 0.05f);

            while (true) {
                Fluid.AddDensity(SIZE / 2, SIZE / 2, 100.0f * (float)rand.NextDouble());
                Fluid.AddVelocity(SIZE / 2, SIZE / 2, rand.Next() % 11 - 5, rand.Next() % 11 - 5);
                Fluid.Step();
                Console.SetCursorPosition(0, 0);
                Console.Write(Fluid.renderDensity());
            }
        }
    }
}