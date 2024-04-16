namespace Photoforge_Server.Services;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Drawing.Imaging;

public class ImageService : IImageService
{

    public Image CreateImage(int width, int height, Color? background)
    {
        Image<Rgba32> image = new(width, height);


        if (background != null)
        {
            image.Mutate(ctx => ctx.BackgroundColor(background.Value));

        }
        return image;


    }

    public void BlurImage(Image image)
    {

        throw new NotImplementedException();
    }

    public void Brightness(Image image)
    {
        throw new NotImplementedException();
    }

    public void MergeImages(IFormFile[] images)
    {
        throw new NotImplementedException();
    }

    public void QucikExport(ImageFormat format)
    {

    }
}

