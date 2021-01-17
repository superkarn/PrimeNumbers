using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers.FrameGenerators
{
    public class BinaryFrameGenerator : IFrameGenator
    {
        int width;
        int height;
        int[] data;

        public BinaryFrameGenerator(int width = 100, int height = 100, int[] data = null)
        {
            this.width = width;
            this.height = height;
            this.data = data;
        }

        public Image<Rgba32> Generate(int wrapWidth)
        {
            // wrapWidth must be <= width
            if (wrapWidth > this.width)
            {
                return null;
            }

            var image = new Image<Rgba32>(this.width, this.height);
            
            return image;
        }
    }
}