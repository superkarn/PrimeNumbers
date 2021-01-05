using System;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace prime_numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataLocation = @".\data\0-100000.txt";
            var outputLocation = @"D:\temp\prime-numbers\output.png";

            var data = LoadData(dataLocation);
            CreateImage(outputLocation, 200, 200, data);
        }

        static int[] LoadData(string fileName)
        {
            // Load the adat from file
            var strings = File.ReadAllLines(fileName);

            // Convert the data to int[]
            return strings.Select(x => Int32.Parse(x)).ToArray();
        }

        static void CreateImage(string fileName, int width, int height, int[] data)
        {
            using (var image = new Image<Rgba32>(width, height))
            {
                // x, y coordinate of the pixels
                var x = 0;
                var y = 0;

                // Get the largest number from data
                var maxNumber = data[data.Length-1];

                // The number of pixels in this image
                var maxPixelCount = width * height;

                var lastPrime = 0;

                // Loop until we run out of prime numbers or pixels
                for (var ii = 0; ii < maxNumber && ii < maxPixelCount; ii++)
                {
                    // Prime numbers
                    if (data.Contains(ii))
                    {
                        // Check for Twin prime
                        var isTwinPrime = (ii == lastPrime+2 || Array.IndexOf(data, ii+2) > -1);

                        // Twin primes are red
                        if (isTwinPrime)
                        {
                            image[x, y] = new Rgba32(255, 0, 0);
                        }
                        // Regular primes are blue
                        else 
                        {
                            image[x, y] = new Rgba32(0, 0, 255);
                        }

                        lastPrime = ii;
                    }
                    
                    // Move the pixel left to right, then top to bottom
                    if (x > width-2) 
                    {
                        x = 0;
                        y++;
                    } 
                    else 
                    {
                        x++;
                    }
                }

                // Create folder if not exist
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                //Console.WriteLine($"Current folder: {System.AppDomain.CurrentDomain.BaseDirectory}");
                //Console.WriteLine($"Output folder:  {Path.GetDirectoryName(fileName)}");

                // Resize the image to make it easier to see.  Leaving height as 0 to keep the same aspect ratio
                image.Mutate(x => x.Resize(width*4, 0));

                // Save the image
                image.Save(fileName);
            }
        }
    }
}
