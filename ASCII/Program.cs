using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ASCII
{
    internal class Program
    {
        private static string[] CHARS = { "@", "%", "#", "*", "+", "=", "-", ":", ",", ".", " " };
        private const string PATH_PROMT = "Please Enter a path to an image:";

        private static Bitmap GetImage(String path)
        {
            try
            {
                return new Bitmap(path);
            }catch (Exception)
            {
                Console.WriteLine("Invalid path entered, Please try again...");
                return null;
            }
        }

        private static Bitmap ResizeImage(Bitmap image)
        {
            //decimal ratio = ((decimal)image.Height / (decimal)image.Width / 1.7m);

            //int newHeight = (int)(100 * ratio);
            //Console.WriteLine(newHeight);
            return new Bitmap(image, new Size(image.Width / 4, image.Height / 4));
        }

        private static Bitmap ToGrayScale(Bitmap image)
        {
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

        private static string ToASCII(Bitmap image)
        {
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

            return pixels;bool 
        }

        private static List<string> SplitString(string s)
        {
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

        private static string CreateFileName(string imageName)
        {
            return imageName.Split('.')[0];
        }

        private static string GetPathFromUserInput()
        {
            Console.WriteLine(PATH_PROMT);
            var path = Console.ReadLine();
            try
            {
                path = Path.GetFullPath(path);
            }
            catch (Exception)
            {
                Console.WriteLine("Please enter a valid path.");
                path = "";
            }
            return path;
        }
        private static void InitializeASCIIConversion()
        {
            string path = GetPathFromUserInput();

            path = path.Trim();
            while (string.IsNullOrEmpty(path))
            {
                path = GetPathFromUserInput();
            }
            var imageFromPath = GetImage(@path);
            if (imageFromPath == null)
            {
                InitializeASCIIConversion();
            }
            var resizedImage = ResizeImage(imageFromPath);
            Bitmap grey = ToGrayScale(resizedImage);

            var imageName = CreateFileName(Path.GetFileName(path));
            var directory = Path.GetDirectoryName(path);
            var newPath = $"{directory}" + "/" +$"{imageName}.txt";
            File.WriteAllLines(newPath, SplitString(ToASCII(grey)));

            Console.WriteLine($"Here's the path to your ASCII image: {path}");
        }
        private static void Main()
        {
            InitializeASCIIConversion();
        }
    }
}