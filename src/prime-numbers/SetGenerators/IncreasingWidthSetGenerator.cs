using System;
using System.Threading.Tasks;
using prime_numbers.FrameGenerators;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers.SetGenerators
{
    ///<summary>
    ///  Generate a gif where the drawing width (not the image width) increases with each frame.
    ///</summary>
    public class IncreasingWidthSetGenerator : ISetGenerator
    {
        int imageWidth;
        int imageHeight;
        int startFrame;
        int endFrame;
        int[] data;

        IFrameGenator frameGenerator;

        public IncreasingWidthSetGenerator(int imageWidth = 100, int imageHeight = 100, int startFrame = 0, int endFrame = 0, int[] data = null)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.startFrame = startFrame;
            this.endFrame = endFrame == 0 ? imageWidth: endFrame;
            this.data = data;

            this.frameGenerator = new DefaultFrameGenerator(imageWidth, imageHeight, data);
        }

        public Image<Rgba32> Generate()
        {
            var gif = new Image<Rgba32>(this.imageWidth, this.imageHeight);

            ImageFrame<Rgba32>[] frames;

            Console.WriteLine($"Generating frames");
            frames = this.GenerateFrames(this.imageWidth, this.imageHeight, this.data);

            // Add each frame to the gif
            for (int ii = this.startFrame; ii < this.endFrame && ii < frames.Length; ii++)
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

        private ImageFrame<Rgba32>[] GenerateFrames(int width, int height, int[] data)
        {
            var frames = new ImageFrame<Rgba32>[width];

            Parallel.For(startFrame, Math.Min(endFrame, width), currentFrame =>
                {
                    // Only calculate when the currentFrame is prime
                    //if (data.Contains(currentFrame))
                    //{
                        Console.Write($".");
                        frames[currentFrame] = this.frameGenerator.Generate(currentFrame).Frames[0];
                    //}
                    //else 
                    //{
                    //    Console.Write($"_");
                    //}
                });

            return frames;
        }
    }
}