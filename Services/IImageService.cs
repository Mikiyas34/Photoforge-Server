namespace Photoforge_Server.Services;

using Microsoft.Identity.Client;
using NetTopologySuite.Geometries;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Buffers;
public interface IImageService
{
    public void CreateFromFile(IFormFile file, string path);
    public bool IsImageSizeValid(IFormFile image);

    Image CreateImage(int width, int height, Color? background);
    public bool IsPointInsidePolygon(Coordinate point, int[] points);
    void MergeImages(Image[] images); 

    void BlurImage(string path, int value);

    void Brightness(string path, float brightness);

    void ClearArea(string path, List<double> points);

    void FillArea(string path, List<double> points);
    Image CloneImage(string path); 


}


