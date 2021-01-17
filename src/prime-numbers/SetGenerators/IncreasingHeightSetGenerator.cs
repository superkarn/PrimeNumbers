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
    public class IncreasingHeightSetGenerator : ISetGenerator
    {
        int imageWidth;
        int imageHeight;
        int startFrame;
        int endFrame;
        int[] data;

        IFrameGenator frameGenerator;

        public IncreasingHeightSetGenerator(int imageWidth = 100, int imageHeight = 100, int startFrame = 0, int endFrame = 100, int[] data = null)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.startFrame = startFrame;
            this.endFrame = endFrame == 0 ? 100: endFrame;
            this.data = data;

            this.frameGenerator = new BinaryFrameGenerator(imageWidth, imageHeight, data);
        }

        public Image<Rgba32> Generate()
        {
            var gif = new Image<Rgba32>(this.imageWidth, this.imageHeight);

            return gif;
        }

        private ImageFrame<Rgba32>[] GenerateFrames(int width, int height, int[] data)
        {
            var frames = new ImageFrame<Rgba32>[width];

            Parallel.For(startFrame, Math.Min(endFrame, width), currentFrame =>
                {
                });

            return frames;
        }
    }
}