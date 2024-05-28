namespace Photoforge_Server.Services;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
public class ImageService : IImageService
{
    public void CreateFromFile(IFormFile file, string path)
    {
        using (var newfile = File.Create(path))
        {
            file.CopyTo(newfile);

        }
    }

    public bool IsImageSizeValid(IFormFile image)
    {
        var MAX_IMAGE_SIZE = 10_485_760; //10Mb

        if (image.Length >= MAX_IMAGE_SIZE)
        {
            return false;
        }

        return true;

    }
    public void BlurImage(string path, int value)
    {

        throw new NotImplementedException();
    }

    public bool IsPointInsidePolygon(Coordinate point, int[] points)
    {

        Coordinate[] polygonVertices = { };
        for (int i = 0; i < points.Length; i += 2)
        {
            polygonVertices.Append(new Coordinate(points[i], points[i + 1]));
        }

        GeometryFactory geometryFactory = new GeometryFactory();
        Polygon polygon = geometryFactory.CreatePolygon(polygonVertices);

        var targetPoint = geometryFactory.CreatePoint(point);

        bool isInside = polygon.Within(targetPoint);

        return isInside;
    }

    public bool IsPointInsidePolygon(PointF point, int[] points)
    {
        bool isInside = false;
        PointF[] polygon = new PointF[points.Length];

        for (int i = 0; i < points.Length; i += 2)
        {
            polygon.Append(new PointF(points[i], points[i + 1]));
        }

        for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
        {
            if (((polygon[i].Y > point.Y) != (polygon[j].Y > point.Y)) && (point.X < (polygon[j].X - polygon[i].X) * (point.Y - polygon[j].Y - polygon[i].Y) + polygon[i].X))
            {
                isInside = !isInside;
            }
        }


        return isInside;
    }
    public void Brightness(string path, float brightness)
    {
        throw new NotImplementedException();
    }

    public void ClearArea(string path, List<double> points)
    {


        throw new NotImplementedException();
    }

    public Image CloneImage(string path)
    {
        var img = Image.Load(path);
        return img;
    }

    public Image CreateImage(int width, int height, Color? background)
    {
        Image<Rgba32> image = new(width, height);


        if (background != null)
        {
            image.Mutate(ctx => ctx.BackgroundColor(background.Value));

        }
        return image;
    }

    public void FillArea(string path, List<double> points)
    {
        throw new NotImplementedException();
    }

    public void MergeImages(Image[] images)
    {
        throw new NotImplementedException();
    }
}

