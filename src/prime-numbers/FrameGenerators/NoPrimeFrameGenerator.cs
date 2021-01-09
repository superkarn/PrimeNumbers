using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers.FrameGenerators
{
    public class NoPrimesFrameGenerator : IFrameGenator
    {
        int width;
        int height;
        int[] data;

        int MaxShades = 16;
        Rgba32[] ColorShades;

        public NoPrimesFrameGenerator(int width = 100, int height = 100, int[] data = null)
        {
            this.width = width;
            this.height = height;
            this.data = data;

            this.GenerateShades();
        }

        public Image<Rgba32> Generate(int wrapWidth)
        {
            // wrapWidth must be <= width
            if (wrapWidth > this.width)
            {
                return null;
            }

            var image = new Image<Rgba32>(this.width, this.height);

            // x, y coordinate of the pixels
            var x = 0;
            var y = 0;

            // Get the largest number from data
            var maxNumber = this.data[this.data.Length-1];

            // The number of pixels in this image
            var maxPixelCount = this.width * this.height;

            // Loop until we run out of prime numbers or pixels
            for (var ii = 0; ii < maxNumber && ii < maxPixelCount && y < this.height; ii++)
            {
                // All numbers
                image[x, y] = this.ColorShades[ii % this.MaxShades];
                
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

        private void GenerateShades()
        {
            this.ColorShades = new Rgba32[this.MaxShades];

            // How much to increment to increase for each shade
            var shadeStep = 255/this.MaxShades;
            byte shadeValue = 0;

            Console.WriteLine($"Generating {this.MaxShades} shades @ {shadeStep} per step.");

            for (var ii = 0; ii < this.MaxShades-1; ii++)
            {
                shadeValue += (byte)shadeStep;
                this.ColorShades[ii] = new Rgba32(shadeValue, shadeValue, shadeValue);
            }
        }
    }
}