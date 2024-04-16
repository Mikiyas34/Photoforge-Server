namespace Photoforge_Server.Services;

using Microsoft.Identity.Client;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Buffers;
public interface IImageService
{
    
    Image CreateImage(int width, int height, Color? background);
    void MergeImages(IFormFile[] images);

    void BlurImage(Image image);

    void Brightness(Image image);
}


