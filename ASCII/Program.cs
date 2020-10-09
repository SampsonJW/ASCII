using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;

namespace ASCII
{
    class Program
    {
        public static string[] CHARS = {"@", "%", "#", "*", "+", "=", "-", ":", ",", ".", " "};
        public static Bitmap GetImage(String path) {
                return new Bitmap(path);
        }

        public static Bitmap ResizeImage(Bitmap image) {
            decimal ratio = ((decimal)image.Height / (decimal)image.Width / 1.75m);
            
            int newHeight = (int)(100 * ratio);
            Console.WriteLine(newHeight);
            return new Bitmap(image, new Size(100, newHeight));
        }

        public static Bitmap ToGrayScale(Bitmap image) {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = image.GetPixel(i, j);

                    byte gray = (byte)(.21 * c.R + .71 * c.G + .071 * c.B);

                    image.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }

            return image;
        }
        public static string ToASCII(Bitmap image) {

            string pixels = "";
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    int c = image.GetPixel(j, i).R;
                    double d = c / 25;
                    double index = Math.Round(d);
                    pixels += CHARS[(int)index];

                }
            }


            return pixels;
        }
        public static void SaveImage(Bitmap bm, String fileName) {
            String path = @"D:\CS-Projects\ASCII\ASCII\";
            Bitmap copy = bm;
            copy.Save(path+fileName+"GS.jpg", ImageFormat.Jpeg);
        }

        public static List<string> SplitString(string s) {
            int len = s.Length;
            List<string> ascii = new List<string>();
            for (int i = 0; i < len; i++)
            {
                if (i % 100 == 0 && i != 0)
                {
                    ascii.Add(s.Substring(i, 100));

                }
            }
            return ascii;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Please Enter a path to image:");
            String path = Console.ReadLine();

            Bitmap grey = ToGrayScale(ResizeImage(GetImage(@path)));

            File.Delete(@"D:\CS-Projects\ASCII\ASCII\ASCII.txt");

            File.WriteAllLines(@"D:\CS-Projects\ASCII\ASCII\ASCII.txt", SplitString(ToASCII(grey)));

            Console.WriteLine(@"Here's the path to your ASCII image: D:\CS-Projects\ASCII\ASCII\ASCII.txt");


        }
    }
}
