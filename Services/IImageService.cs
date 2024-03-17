namespace Photoforge_Server.Services;

using Microsoft.Identity.Client;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Buffers;
public interface IImageService
{
    void MergeImages(IFormFile[] images);


}


