namespace Photoforge_Server.Services;
using System.Text;
using Microsoft.AspNetCore.Http;
using System.Buffers.Binary;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;

public class ImageService : IImageService
{
    public ImageService() {
        var json = new { Name = "bob", Age = 19};

        var jsonStr = JsonConvert.SerializeObject(json);
        var binaryData = Encoding.UTF8.GetBytes(jsonStr);

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

