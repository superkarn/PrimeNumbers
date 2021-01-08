using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers.FrameGenerators
{
    ///<summary>
    ///  FrameGenerators generate frames (images)
    ///</summary>
    public interface IFrameGenator
    {
        Image<Rgba32> Generate(int wrapWidth);        
    } 
}