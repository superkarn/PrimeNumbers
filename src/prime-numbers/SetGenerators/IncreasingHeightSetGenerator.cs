using System;
using System.Threading.Tasks;
using prime_numbers.FrameGenerators;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

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
        int maxNumber;
        int[] data;

        IFrameGenator frameGenerator;

        public IncreasingHeightSetGenerator(int imageWidth = 20, int imageHeight = 100, int startFrame = 0, int endFrame = 100, int[] data = null)
        {
            this.imageWidth = imageWidth;
            this.imageHeight = imageHeight;
            this.startFrame = startFrame;
            this.endFrame = endFrame == 0 ? 100: endFrame;
            this.maxNumber = (int)Math.Pow(2, this.endFrame);
            this.data = data;

            this.frameGenerator = new BinaryFrameGenerator(imageWidth, this.maxNumber, endFrame, data);
        }

        public Image<Rgba32> Generate()
        {
            var gif = new Image<Rgba32>(this.imageWidth, this.imageHeight);

            ImageFrame<Rgba32>[] frames;

            Console.WriteLine($"Generating frames");
            frames = this.GenerateFrames(this.imageWidth, this.imageHeight, this.maxNumber, this.data);

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

            GifFrameMetadata frameMetaData;

            // Each frame should hold for 1 second
            for (var ii = 0; ii < gif.Frames.Count; ii++)
            {
                if (gif.Frames[ii] == null)
                {
                    Console.WriteLine($"Frame #{ii} is null.");
                }
                frameMetaData = gif.Frames[ii].Metadata.GetFormatMetadata(GifFormat.Instance);
                frameMetaData.FrameDelay = 100; 
            }
            
            // Make the last frame last a while (2 seconds) before looping
            //frameMetaData = gif.Frames[gif.Frames.Count-1].Metadata.GetFormatMetadata(GifFormat.Instance);
            //frameMetaData.FrameDelay = 300;

            return gif;
        }

        private ImageFrame<Rgba32>[] GenerateFrames(int width, int height, int maxNumber, int[] data)
        {
            var frames = new ImageFrame<Rgba32>[this.endFrame];

            // TODO optimize this
            // Currently drawing a large image, then cropping.
            // Optimize to only draw what is needed after cropping.
            Parallel.For(startFrame, this.endFrame+1, currentFrame =>
                {
                    var image = this.frameGenerator.Generate(currentFrame);
                    image.Mutate(x => x.Crop(new Rectangle(0, maxNumber-height, 20, height)));
                    frames[currentFrame-1] = image.Frames[0];
                    Console.WriteLine($"    Completed frame {currentFrame}");
                });

            Console.WriteLine($"Total Frames: {frames.Length}");
            return frames;
        }
    }
}