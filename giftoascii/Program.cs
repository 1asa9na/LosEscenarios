using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Collections.Generic;
namespace Program {
    public class Program {
        public static void Main(string[] args) {
            string path = args[0];
            int h = Convert.ToInt32(args[1]);
            int w = Convert.ToInt32(args[2]);
            Image img = Image.FromFile(path);
            Bitmap[] bitmapArray = GetFramesFromAnimatedGIF(img);
            int framecount = img.GetFrameCount(FrameDimension.Time);
            char[][] screen = new char[framecount][];
            for (int k = 0; k < screen.Length; k++) {
                string ascii = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/|()1{}[]?-_+~<>i!lI;:,'.";
                byte[,,] rgb = BitmapToByteRgbQ(bitmapArray[k]);
                int[,] grad = RGMBitmapToGradArray(rgb, h, w);
                screen[k] = new char[grad.GetLength(0) * grad.GetLength(1)];
                for (int i = 0; i < grad.GetLength(0); i++) {
                    for (int j = 0; j < grad.GetLength(1); j++) {
                        screen[k][i * grad.GetLength(1) + j] = ascii[grad[i, j] / 4];
                    }
                }
            }
            int n = 0;
            Console.Title = $"Set resolution: {(int)Math.Ceiling((float)img.Height / h)}H x {(int)Math.Ceiling((float)img.Width / w)}W";
            while (true) {
                Thread.Sleep(25);
                Console.SetCursorPosition(0, 0);
                Console.Write(screen[n]);
                n = ++n % framecount;
            }
        }

        public static Bitmap[] GetFramesFromAnimatedGIF(Image IMG) {
            List<Bitmap> IMGs = new List<Bitmap>();
            int Length = IMG.GetFrameCount(FrameDimension.Time);

            for (int i = 0; i < Length; i++) {
                IMG.SelectActiveFrame(FrameDimension.Time, i);
                IMGs.Add(new Bitmap(IMG));
            }

            return IMGs.ToArray();
        }
        public unsafe static byte[,,] BitmapToByteRgbQ(Bitmap bmp) {
            int width = bmp.Width;
            int height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
            PixelFormat.Format24bppRgb);
            try {
                byte* curpos;
                fixed (byte* _res = res) {
                    byte* _r = _res, _g = _res + width * height, _b = _res + 2 * width * height;
                    for (int h = 0; h < height; h++) {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++) {
                            *_b = *(curpos++); ++_b;
                            *_g = *(curpos++); ++_g;
                            *_r = *(curpos++); ++_r;
                        }
                    }
                }
            }
            finally {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        public unsafe static int[,] RGMBitmapToGradArray(byte[,,] bmp, int a, int b) {
            int hn = a;
            int wn = b;
            int height = bmp.GetLength(1);
            int width = bmp.GetLength(2);
            int[,] res = new int[(int)Math.Ceiling((float)height / hn), (int)Math.Ceiling((float)width / wn)];
            for (int h = 0; h < height; h += hn) {
                for (int w = 0; w < width; w += wn) {
                    int hhh = (height - h < hn) ? height - h : hn;
                    int www = (width - w < wn) ? width - w : wn;
                    for (int hh = 0; hh < hhh; hh++) {
                        for (int ww = 0; ww < www; ww++) {
                            res[h / hn, w / wn] += (int)(bmp[0, h + hh, w + ww] + bmp[1, h + hh, w + ww] + bmp[2, h + hh, w + ww]) / 3;
                        }
                    }
                    res[h / hn, w / wn] /= hhh * www;
                }
            }
            return res;
        }
    }
}