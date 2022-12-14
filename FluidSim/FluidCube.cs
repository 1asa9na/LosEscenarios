using System;
using Pastel;

namespace FluidSim {
    class FluidCube {
        const string symbs = "$@B%8&WM#*oahkbdpqwmZO0LCJUYXzcvunxrjft/|()1{}[]?-_+~<>i!lI;:,'.";

        string[] colors = {
            "#e63d26",
            "#ea6739",
            "#eda169",
            "#f0d2a8",
            "#65a098",
            "#038695",
            "#026a8a",
            "#10234b",
        };

        readonly float dt;
        readonly float diff;
        readonly float visc;

        readonly float[,] s;
        readonly float[,] density;

        readonly float[,] Vx;
        readonly float[,] Vy;

        readonly float[,] Vx0;
        readonly float[,] Vy0;

        public FluidCube(int diffusion, int viscosity, float dt) {
            this.dt = dt;
            this.diff = diffusion;
            this.visc = viscosity;

            this.s = new float[Field.SIZE, Field.SIZE];
            this.density = new float[Field.SIZE, Field.SIZE];

            this.Vx = new float[Field.SIZE, Field.SIZE];
            this.Vy = new float[Field.SIZE, Field.SIZE];

            this.Vx0 = new float[Field.SIZE, Field.SIZE];
            this.Vy0 = new float[Field.SIZE, Field.SIZE];
        }

        public void AddDensity(int x, int y, float amount) {
            this.density[x, y] += amount;
        }

        public void AddVelocity(int x, int y, float amountX, float amountY) {
            this.Vx[x, y] += amountX;
            this.Vy[x, y] += amountY;
        }

        public static void diffuse(int b, float[,] x, float[,] x0, float diff, float dt) {
            float a = dt * diff * (Field.SIZE - 2) * (Field.SIZE - 2);
            lin_solve(b, x, x0, a, 1 + 6 * a);
        }

        public static void lin_solve(int b, float[,] x, float[,] x0, float a, float c) {
            float cRecip = 1.0f / c;
            for (int k = 0; k < Field.ITER; k++) {
                for (int j = 1; j < Field.SIZE - 1; j++) {
                    for (int i = 1; i < Field.SIZE - 1; i++) {
                        x[i, j] = (x0[i, j]
                            + a * (x[i + 1, j]
                            + x[i - 1, j]
                            + x[i, j + 1]
                            + x[i, j - 1]
                            )) * cRecip;
                    }
                }
                set_bnd(b, x);
            }
        }

        public static void project(float[,] velocX, float[,] velocY, float[,] p, float[,] div) {
            for (int j = 1; j < Field.SIZE - 1; j++) {
                for (int i = 1; i < Field.SIZE - 1; i++) {
                    div[i, j] = -0.5f * (
                    velocX[i + 1, j]
                    - velocX[i - 1, j]
                    + velocY[i, j + 1]
                    - velocY[i, j - 1]
                    ) / Field.SIZE;
                    p[i, j] = 0;
                }
            }
        
        set_bnd(0, div);
        set_bnd(0, p);
        lin_solve(0, p, div, 1, 6);

            for (int j = 1; j < Field.SIZE - 1; j++) {
                for (int i = 1; i < Field.SIZE - 1; i++) {
                    velocX[i, j] -= 0.5f * (p[i + 1, j] - p[i - 1, j]) * Field.SIZE;
                    velocY[i, j] -= 0.5f * (p[i, j + 1] - p[i, j - 1]) * Field.SIZE;
                }
            }
            set_bnd(1, velocX);
            set_bnd(2, velocY);
        }

        public static void advect(int b, float[,] d, float[,] d0, float[,] velocX, float[,] velocY, float dt) {
            float i0, i1, j0, j1;

            float dtx = dt * (Field.SIZE - 2);
            float dty = dt * (Field.SIZE - 2);

            float s0, s1, t0, t1;
            float tmp1, tmp2, x, y;

            float Nfloat = Field.SIZE;
            float ifloat, jfloat;
            int i, j;

            for (j = 1, jfloat = 1; j < Field.SIZE - 1; j++, jfloat++) {
                for (i = 1, ifloat = 1; i < Field.SIZE - 1; i++, ifloat++) {
                    tmp1 = dtx * velocX[i, j];
                    tmp2 = dty * velocY[i, j];
                    x = ifloat - tmp1;
                    y = jfloat - tmp2;

                    if (x < 0.5f) x = 0.5f;
                    if (x > Nfloat + 0.5f) x = Nfloat + 0.5f;
                    i0 = (float)Math.Floor(x);
                    i1 = i0 + 1.0f;
                    if (y < 0.5f) y = 0.5f;
                    if (y > Nfloat + 0.5f) y = Nfloat + 0.5f;
                    j0 = (float)Math.Floor(y);
                    j1 = j0 + 1.0f;

                    s1 = x - i0;
                    s0 = 1.0f - s1;
                    t1 = y - j0;
                    t0 = 1.0f - t1;

                    int i0i = (int)i0;
                    int i1i = (int)i1;
                    int j0i = (int)j0;
                    int j1i = (int)j1;

                    d[i, j] = 
                        s0 * (t0 * d0[i0i, j0i]
                        + t1 * d0[i0i, j1i])
                        + s1 * (t0 * d0[i1i, j0i]
                        + t1 * d0[i1i, j1i]);
                }
            }
            set_bnd(b, d);
        }

        static void set_bnd(int b, float[,] x) {
            for (int i = 1; i < Field.SIZE - 1; i++) {
                x[i, 0] = b == 2 ? -x[i, 1] : x[i, 1];
                x[i, Field.SIZE - 1] = b == 2 ? -x[i, Field.SIZE - 2] : x[i, Field.SIZE - 2];
            }
            for (int j = 1; j < Field.SIZE - 1; j++) {
                x[0, j] = b == 1 ? -x[1, j] : x[1, j];
                x[Field.SIZE - 1, j] = b == 1 ? -x[Field.SIZE - 2, j] : x[Field.SIZE - 2, j];
            }

            x[0, 0] = 0.33f * (x[1, 0]
                + x[0, 1]
                + x[0, 0]);
            x[0, Field.SIZE - 1] = 0.33f * (x[1, Field.SIZE - 1]
                + x[0, Field.SIZE - 2]
                + x[0, Field.SIZE - 1]);
            x[Field.SIZE - 1, 0] = 0.33f * (x[Field.SIZE - 2, 0]
                + x[Field.SIZE - 1, 1]
                + x[Field.SIZE - 1, 0]);
            x[Field.SIZE - 1, Field.SIZE - 1] = 0.33f * (x[Field.SIZE - 2, Field.SIZE - 1]
                + x[Field.SIZE - 1, Field.SIZE - 2]
                + x[Field.SIZE - 1, Field.SIZE - 1]);
        }

        public void Step() {
            float visc = this.visc;
            float diff = this.diff;
            float dt = this.dt;
            float[,] Vx = this.Vx;
            float[,] Vy = this.Vy;
            float[,] Vx0 = this.Vx0;
            float[,] Vy0 = this.Vy0;
            float[,] s = this.s;
            float[,] density = this.density;

            diffuse(1, Vx0, Vx, visc, dt);
            diffuse(2, Vy0, Vy, visc, dt);

            project(Vx0, Vy0, Vx, Vy);

            advect(1, Vx, Vx0, Vx0, Vy0, dt);
            advect(2, Vy, Vy0, Vx0, Vy0, dt);

            project(Vx, Vy, Vx0, Vy0);

            diffuse(0, s, density, diff, dt);
            advect(0, density, s, Vx, Vy, dt);
        }

        public char[] renderDensity() {
            char[] densityMap = new char[Field.SIZE * Field.SIZE * Field.SCALE * Field.SCALE * Field.fontAspect];
            for (int i = 0; i < Field.SIZE; i++) {
                for (int j = 0; j < Field.SIZE; j++) {
                    float x = i * Field.fontAspect * Field.SCALE;
                    float y = j * Field.SCALE;
                    float d = this.density[i, j];
                    for(int ii = 0; ii < Field.fontAspect * Field.SCALE; ii++) {
                        for (int jj = 0; jj < Field.SCALE; jj++) {
                            densityMap[(j + jj) * Field.SIZE * Field.fontAspect * Field.SCALE + i * Field.fontAspect + ii] = symbs[63 - (int)d % 64];
                        }
                    }
                }
            }
            return densityMap;
        }
    };
}