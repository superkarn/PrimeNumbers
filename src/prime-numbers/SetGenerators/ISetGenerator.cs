using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace prime_numbers.SetGenerators
{
    ///<summary>
    ///  SetGenerators generate gifs, which can contain many frames generated
    ///  by FrameGenerators
    ///</summary>
    public interface ISetGenerator
    {
        Image<Rgba32> Generate();
    } 
}