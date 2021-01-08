using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prime_numbers.Generators;
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
            var frameGenerator = new DefaultGenerator();

            Parallel.For(startFrame, Math.Min(endFrame, width), currentFrame =>
                {
                    // Only calculate when the currentFrame is prime
                    if (data.Contains(currentFrame))
                    {
                        Console.Write($".");
                        frames[currentFrame] = frameGenerator.CreateFrame(currentFrame, width, height, data).Frames[0];
                    }
                    else 
                    {
                        Console.Write($"_");
                    }
                });

            return frames;
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
