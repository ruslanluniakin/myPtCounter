using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms.VisualStyles;

namespace PtCounter.model
{
    public static class BitmapCounter
    {

        private unsafe static byte[,,] BitmapToByteRgbQ(Bitmap bmp)
        {
            int width = bmp.Width,
                height = bmp.Height;
            byte[,,] res = new byte[3, height, width];
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly,
                PixelFormat.Format24bppRgb);
            try
            {
                byte* curpos;
                fixed (byte* _res = res)
                {
                    byte* _r = _res, _g = _res + width * height, _b = _res + 2 * width * height;
                    for (int h = 0; h < height; h++)
                    {
                        curpos = ((byte*)bd.Scan0) + h * bd.Stride;
                        for (int w = 0; w < width; w++)
                        {
                            *_b = *(curpos++); ++_b;
                            *_g = *(curpos++); ++_g;
                            *_r = *(curpos++); ++_r;
                        }
                    }
                }
            }
            finally
            {
                bmp.UnlockBits(bd);
            }
            return res;
        }

        public static int BitmapCount(Bitmap bmp, Rectangle rectangle) //new Rectangle(287, 183, 57, 10)
        {
            int m = rectangle.Width;
            int n = rectangle.Height;

            int counter = 0;

            bmp = bmp.Clone(rectangle, bmp.PixelFormat);

            var b = BitmapToByteRgbQ(bmp); // получение byte[,,]

            byte[,] bin = new byte[10, 57];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    bin[i, j] = ((b[0, i, j] + b[1, i, j] + b[2, i, j]) / 3) > 145 ? (byte)1 : (byte)0;
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (bin[i, j] == 1)
                    {
                        counter++;
                        Fill(bin, n, m, i, j); // заполнение подсчитанных
                    }
                }
            }

            return counter;
        }
        private static bool Check(int n, int m, int i, int j)
        {
            if (i < 0 || j < 0 || i >= n || j >= m)
                return false;
            return true;
        }

        private static byte[,] Fill(byte[,] Image, int n, int m, int i, int j)
        {
            Image[i, j] = 2;
            if (Check(n, m, i - 1, j) && Image[i - 1, j] == 1)
            {
                Fill(Image, n, m, i - 1, j);
            }
            if (Check(n, m, i, j - 1) && Image[i, j - 1] == 1)
            {
                Fill(Image, n, m, i, j - 1);
            }
            if (Check(n, m, i + 1, j) && Image[i + 1, j] == 1)
            {
                Fill(Image, n, m, i + 1, j);
            }

            if (Check(n, m, i, j + 1) && Image[i, j + 1] == 1)
            {
                Fill(Image, n, m, i, j + 1);
            }
            return Image;
        }
    }
}
