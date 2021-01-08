using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace prime_numbers
{
    class Program
    {
        // TODO Convert these to input parameters
        static int imageWidth = 100;
        static int imageHeight = 100;
        static int startFrame = 0;
        static int endFrame = imageWidth; // Must be <= imageWidth

        static readonly Rgba32 BLACK = new Rgba32(0, 0, 0);
        static readonly Rgba32 BLUE = new Rgba32(0, 0, 255);
        static readonly Rgba32 CYAN = new Rgba32(0, 255, 255);
        static readonly Rgba32 GRAY100 = new Rgba32(225, 225, 225);
        static readonly Rgba32 GRAY10 = new Rgba32(240, 240, 240);
        static readonly Rgba32 GREEN = new Rgba32(0, 255, 0);
        static readonly Rgba32 PINK = new Rgba32(255, 0, 255);
        static readonly Rgba32 RED = new Rgba32(255, 0, 0);
        static readonly Rgba32 WHITE = new Rgba32(255, 255, 255);
        static readonly Rgba32 YELLOW = new Rgba32(255, 255, 0);

        static void Main(string[] args)
        {
            var timer = new Stopwatch();
            timer.Start();

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

            Console.WriteLine($"------------------------------");
            Console.WriteLine($"Parameters");
            Console.WriteLine($"    Image dimension: {imageWidth}x{imageHeight}");
            Console.WriteLine($"    Frames: {startFrame} to {endFrame}");
            Console.WriteLine($"");

            var data = LoadData(dataLocations);
            using(var gif = CreateGif(imageWidth, imageHeight, data))
            {
                SaveImage(outputLocation, gif);
            }
            
            timer.Stop();

            Console.WriteLine($"");
            Console.WriteLine($"Total runtime: {timer.Elapsed.ToString("mm':'ss'.'fff")}");
            Console.WriteLine($"------------------------------");
        }

        static Image<Rgba32> CreateGif(int width, int height, int[] data)
        {
            var gif = new Image<Rgba32>(width, height);

            ImageFrame<Rgba32>[] frames;

            Console.Write($"Creating frames ");
            frames = CreateGifFrames_Parallel(width, height, data);

            // Add each frame to the gif
            for (int ii = startFrame; ii < endFrame && ii < frames.Length; ii++)
            {
                if (frames[ii] != null)
                {
                    gif.Frames.AddFrame(frames[ii]);
                }
            }
            Console.WriteLine($"");

            // Make the gif loop indefinitely
            var gifMetaData = gif.Metadata.GetFormatMetadata(GifFormat.Instance);
            gifMetaData.RepeatCount = 0;
            gifMetaData.ColorTableMode = GifColorTableMode.Global;

            // Make the last frame last a while (2 seconds) before looping
            var frameMetaData = gif.Frames[gif.Frames.Count-1].Metadata.GetFormatMetadata(GifFormat.Instance);
            frameMetaData.FrameDelay = 200;

            return gif;
        }
               
        /// <summary>
        /// Using Parallel.For to speed things up, but have to make sure the result are in order
        /// </summary>
        static ImageFrame<Rgba32>[] CreateGifFrames_Parallel(int width, int height, int[] data)
        {
            var frames = new ImageFrame<Rgba32>[width];

            Parallel.For(startFrame, Math.Min(endFrame, width), currentFrame =>
                {
                    // Only calculate when the currentFrame is prime
                    if (data.Contains(currentFrame))
                    {
                        Console.Write($".");
                        frames[currentFrame] = CreateImage(currentFrame, width, height, data).Frames[0];
                    }
                    else 
                    {
                        Console.Write($"_");
                    }
                });

            return frames;
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
                        image[x, y] = BLUE;
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
                    // TODO Optimize drawing the grid
                    // Color Grid
                         if (x % 100 == 0) image[x, y] = GRAY100;
                    else if (y % 100 == 0) image[x, y] = GRAY100;
                    else if (x %  10 == 0) image[x, y] = GRAY10;
                    else if (y %  10 == 0) image[x, y] = GRAY10;
                    else                   image[x, y] = WHITE;
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
            
            Console.WriteLine($"Loading prime numbers");
            foreach (var fileName in fileNames)
            {
                //Console.WriteLine($"    {fileName}");
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
