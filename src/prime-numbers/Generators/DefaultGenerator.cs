using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers.Generators
{
    public class DefaultGenerator : IFrameGenator
    {
        public Image<Rgba32> CreateFrame(int wrapWidth, int width, int height, int[] data)
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
                        image[x, y] = Colors.BLUE;
                    }
                    // Regular primes
                    else 
                    {
                        image[x, y] = Colors.WHITE;
                    }

                    lastPrime = ii;
                }
                // Non prime numbers 
                else
                {
                    // TODO Optimize drawing the grid
                    // Color Grid
                         if (x % 100 == 0) image[x, y] = Colors.GRAY100;
                    else if (y % 100 == 0) image[x, y] = Colors.GRAY100;
                    else if (x %  10 == 0) image[x, y] = Colors.GRAY10;
                    else if (y %  10 == 0) image[x, y] = Colors.GRAY10;
                    else                   image[x, y] = Colors.WHITE;
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
    }
}