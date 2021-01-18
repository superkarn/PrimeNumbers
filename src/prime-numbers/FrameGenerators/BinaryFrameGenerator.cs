using System;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers.FrameGenerators
{
    public class BinaryFrameGenerator : IFrameGenator
    {
        const char ONE = '1';
        const char ZERO = '-';

        int width;
        int height;
        int endFrame;
        int[] data;

        public BinaryFrameGenerator(int width = 100, int height = 100, int endFrame = 8, int[] data = null)
        {
            this.width = width;
            this.height = height;
            this.endFrame = endFrame;
            this.data = data;
        }

        public Image<Rgba32> Generate(int currentFrame)
        {
            var endEntry = (int)Math.Pow(2, currentFrame);
            var yOffSet = (int)(Math.Pow(2, this.endFrame) - endEntry);
            var image = new Image<Rgba32>(this.width, this.height);

            for (var y = 1; y < endEntry && y < this.height; y++)
            {
                //Console.Write($".");

                // Padding 20 here because 20 is the max digits we have in the data folder for now.
                var currentValue = Convert.ToString(y, 2).PadLeft(20, ' ');

                if (this.data.Contains(y))
                {
                    for (var x = 0; x < currentValue.Length; x++)
                    {
                        if (currentValue[x] == ONE)
                        {
                            image[x, y+yOffSet] = Colors.RED;
                        }
                    }
                }
                else 
                {
                    //Console.WriteLine($"{y}: ---");
                    for (var x = 0; x < currentValue.Length; x++)
                    {
                        if (currentValue[x] == ONE)
                        {
                            image[x, y+yOffSet] = Colors.BLUE;
                        }
                    }
                }
            }
            //Console.WriteLine($"");
                
            return image;
        }

        public Image<Rgba32> Generate2(int startEntry)
        {
            var image = new Image<Rgba32>(this.width, this.height);

            for (var y = startEntry; y < startEntry + this.height; y++)
            {
                var currentValue = Convert.ToString(this.data[y], 2);

                for (var x = 0; x < currentValue.Length; x++)
                {
                    if (currentValue[x] == ONE)
                    {
                        image[x, y] = Colors.BLUE;
                    }
                }
            }
            
            return image;
        }
    }
}