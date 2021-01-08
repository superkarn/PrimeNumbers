using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers.Generators
{
    public interface IFrameGenator
    {
        Image<Rgba32> CreateFrame(int wrapWidth);        
    } 
}