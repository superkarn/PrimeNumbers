using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace prime_numbers
{
    class Program
    {
        static readonly Rgba32 BLUE = new Rgba32(0, 0, 255);
        static readonly Rgba32 GREEN = new Rgba32(0, 255, 0);
        static readonly Rgba32 RED = new Rgba32(255, 0, 0);
        static readonly Rgba32 WHITE = new Rgba32(255, 255, 255);

        static void Main(string[] args)
        {
            var imageWidth = 500;
            var imageHeight = 500;
            string[] dataLocations = { 
                @".\data\primes-to-100k.txt", 
                @".\data\primes-to-200k.txt", 
                @".\data\primes-to-300k.txt", 
                @".\data\primes-to-400k.txt", 
                @".\data\primes-to-500k.txt", 
                @".\data\primes-to-600k.txt", 
                @".\data\primes-to-700k.txt", 
                @".\data\primes-to-800k.txt", 
                @".\data\primes-to-900k.txt", 
                @".\data\primes-to-1000k.txt",
            };
            var outputLocation = $"D:\\temp\\prime-numbers\\{imageWidth}x{imageHeight}.gif";

            var data = LoadData(dataLocations);
            using(var gif = CreateGif(imageWidth, imageHeight, data))
            {
                SaveImage(outputLocation, gif);
            }
            
            Console.WriteLine($"");
        }

        static Image<Rgba32> CreateGif(int width, int height, int[] data)
        {
            var gif = new Image<Rgba32>(width, height);

            Console.Write($"Creating frames ");
            for (int ii = 3; ii < width; ii++) 
            {
                Console.Write($".");
                var image = CreateImage(ii, width, height, data).Frames[0];

                var frameMetaData = image.Metadata.GetFormatMetadata(GifFormat.Instance);
                frameMetaData.FrameDelay = 10;

                gif.Frames.AddFrame(image);
            }
            Console.WriteLine($"");

            var gifMetaData = gif.Metadata.GetFormatMetadata(GifFormat.Instance);
            gifMetaData.RepeatCount = 0;

            return gif;
        }

        static Image<Rgba32> CreateImage(int wrapWidth, int width, int height, int[] data)
        {
            // wrapWidth must be <= width
            if (wrapWidth > width)
            {
                return null;
            }

            var image = new Image<Rgba32>(width, height);

            // x, y coordinate of the pixels
            var x = 0;
            var y = 0;

            // Get the largest number from data
            var maxNumber = data[data.Length-1];

            // The number of pixels in this image
            var maxPixelCount = width * height;

            var lastPrime = 0;

            // Loop until we run out of prime numbers or pixels
            for (var ii = 0; ii < maxNumber && ii < maxPixelCount && y < height; ii++)
            {
                //Console.WriteLine($"y,x: {y}, {x}");

                // Prime numbers
                if (data.Contains(ii))
                {
                    // Check for Twin prime
                    var isTwinPrime = (ii == lastPrime+2 || Array.IndexOf(data, ii+2) > -1);

                    // Twin primes
                    if (isTwinPrime)
                    {
                        image[x, y] = RED;
                    }
                    // Regular primes
                    else 
                    {
                        image[x, y] = WHITE;
                    }

                    lastPrime = ii;
                }
                // Non prime numbers 
                else
                {
                    image[x, y] = WHITE;
                }
                
                // Move the pixel left to right, then top to bottom
                if (x > wrapWidth-2) 
                {
                    x = 0;
                    y++;
                } 
                else 
                {
                    x++;
                }
            }
            
            return image;
        }

        /// <summary>
        /// Load prime number from files
        /// </summary>
        static int[] LoadData(string[] fileNames)
        {
            var data = new List<int>();
            
            Console.WriteLine($"Loading data from");
            foreach (var fileName in fileNames)
            {
                Console.WriteLine($"    {fileName}");
                var strings = File.ReadAllLines(fileName);
                data.AddRange(strings.Select(x => Int32.Parse(x)).ToList());
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
