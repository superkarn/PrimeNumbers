using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using prime_numbers.SetGenerators;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers
{
    class Program
    {
        // TODO Convert these to input parameters
        static int imageWidth = 20;
        static int imageHeight = 256;
        static int startFrame = 6;
        static int endFrame = 20; // 0 = default; depends on the SetGenerator type
        static int dataBaseType = 10; // 2 = binary; 10 = decimal

        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();

            string[] decimalDataLocations = { 
                @"..\..\data\primes-to-100k.txt", 
                @"..\..\data\primes-to-200k.txt", 
                @"..\..\data\primes-to-300k.txt", 
                @"..\..\data\primes-to-400k.txt", 
                @"..\..\data\primes-to-500k.txt", 
                @"..\..\data\primes-to-600k.txt", 
                @"..\..\data\primes-to-700k.txt", 
                @"..\..\data\primes-to-800k.txt", 
                @"..\..\data\primes-to-900k.txt", 
                @"..\..\data\primes-to-1000k.txt",
            };

            string[] binaryDataLocations = { 
                @"..\..\data\binary\primes-to-100k.txt", 
                //@"..\..\data\binary\primes-to-200k.txt", 
                //@"..\..\data\binary\primes-to-300k.txt", 
                //@"..\..\data\binary\primes-to-400k.txt", 
                //@"..\..\data\binary\primes-to-500k.txt", 
                //@"..\..\data\binary\primes-to-600k.txt", 
                //@"..\..\data\binary\primes-to-700k.txt", 
                //@"..\..\data\binary\primes-to-800k.txt", 
                //@"..\..\data\binary\primes-to-900k.txt", 
                //@"..\..\data\binary\primes-to-1000k.txt",
            };
            var outputLocation = $"D:\\temp\\prime-numbers\\{imageWidth}x{imageHeight}.gif";

            Console.WriteLine($"------------------------------");
            Console.WriteLine($"Parameters");
            Console.WriteLine($"    Image dimension: {imageWidth}x{imageHeight}");
            Console.WriteLine($"    Frames: {startFrame} to {endFrame}");
            Console.WriteLine($"");

            ISetGenerator setGenerator;
            switch (dataBaseType)
            {
                case 2:
                    throw new NotImplementedException();

                case 10:
                default:
                    var decimalData = LoadDecimalData(decimalDataLocations);
                    setGenerator = new IncreasingHeightSetGenerator(imageWidth, imageHeight, startFrame, endFrame, decimalData);
                    break;
            }
            

            using(var gif = setGenerator.Generate())
            {
                SaveImage(outputLocation, gif);
            }
            
            timer.Stop();

            Console.WriteLine($"");
            Console.WriteLine($"Total runtime: {timer.Elapsed.ToString("mm':'ss'.'fff")}");
            Console.WriteLine($"------------------------------");
        }

        static int[] LoadDecimalData(string[] fileNames)
        {
            var data = new List<int>();
            
            Console.WriteLine($"Loading prime numbers (decimal)");
            foreach (var fileName in fileNames)
            {
                //Console.WriteLine($"    {fileName}");
                var strings = File.ReadAllLines(fileName);
                data.AddRange(strings.Select(x => Int32.Parse(x)).ToList());
            }

            return data.ToArray();
        }

        static string[] LoadBinaryData(string[] fileNames)
        {
            var data = new List<string>();
            
            Console.WriteLine($"Loading prime numbers (binary)");
            foreach (var fileName in fileNames)
            {
                //Console.WriteLine($"    {fileName}");
                var strings = File.ReadAllLines(fileName);
                data.AddRange(strings.Select(x => x).ToList());
            }

            return data.ToArray();
        }

        static void SaveImage(string fileName, Image<Rgba32> image)
        {
            // Create folder if not exist
            Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            // Resize the image to make it easier to see.  Leaving height as 0 to keep the same aspect ratio
            //image.Mutate(x => x.Resize(width*4, 0));

            Console.WriteLine($"Saving result to {Path.GetDirectoryName(fileName)}");

            // Save the image
            image.Save(fileName);
        }
    }
}
