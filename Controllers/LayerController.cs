using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Photoforge_Server.Data;
using Photoforge_Server.Models;
using Photoforge_Server.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Normalization;
using System.Drawing.Drawing2D;
using System.Text.Encodings.Web;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace Photoforge_Server.Controllers;


[Route("layers")]
[ApiController]
public class LayerController : ControllerBase
{
    private readonly PhotoforgeDbContext _context;
    private readonly IImageService _imageService;
    public LayerController(PhotoforgeDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }

    [HttpPost]
    [Route("{projectId}/{id}/selection/layer-via-copy")]
    public async Task<IActionResult> LayerViaCopy(string projectId, string id, LayerFromSelectionRequest request)
    {

        var layer = GetLayer(projectId, id);

        if (layer == null)
        {
            return NotFound("Layer not found.");
        }


        var path = GetImagePath(layer);

        if (!System.IO.File.Exists(path))
        {
            return BadRequest("Image not found");
        }


        //get the image 
        Image<Rgba32> image = Image.Load<Rgba32>(path);






        // Check if the point is inside the polygon
        var newImage = new Image<Rgba32>(image.Bounds.Width, image.Bounds.Height);
        Coordinate[] insidePoints = { };
        image.ProcessPixelRows(accessor =>
        {
            Rgba32 transparent = Color.Transparent;

            for (int y = 0; y < accessor.Height; y++)
            {
                Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                for (int x = 0; x < pixelRow.Length; x++)
                {
                    ref Rgba32 pixel = ref pixelRow[x];

                    var point = new Coordinate(x, y);

                    bool isInside = _imageService.IsPointInsidePolygon(point, request.Points);

                    if (isInside)
                    {
                        insidePoints.Append(point);
                        newImage[x, y] = pixel;
                    }
                }
            }

        });

        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
        string imageExt = GetImageFileExtension(layer.Url);

        string storedImageName = $"{Guid.NewGuid()}{imageExt}";
        try
        {
            var newImageLayer = CreateLayer(layer.ProjectId.ToString(), "Copy " + layer.Name, baseUrl + "/images/" + storedImageName, storedImageName);
            string newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", storedImageName);
            newImage.Save(newImagePath);

        }
        catch (Exception)
        {

            throw;
        }

        return Ok(insidePoints.ToList());
    }

    private string GetImageFileExtension(string name)
    {
        return name[name.LastIndexOf('.')..];
    }

    private async Task<Layer> CreateLayer(string projectId, string name, string url, string storedImageName)
    {
        var layer = new Layer(name, Guid.Parse(projectId), url);
        layer.StroredImageName = storedImageName;
        await _context.Layers.AddAsync(layer);
        await _context.SaveChangesAsync();
        return layer;
    }

    private static string GetImagePath(Layer layer)
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "Uploads", layer.StroredImageName);
    }

    private Layer? GetLayer(string projectId, string id)
    {
        return _context.Layers.FirstOrDefault(l => l.ProjectId == Guid.Parse(projectId) && l.Id == Guid.Parse(id));
    }

    [HttpPost]
    [Route("{projectId}/merge")]
    public IActionResult MergeLayers(string projectId)
    {
        //get all layers of that project 
        //merge and delete the old layers
        return Ok(projectId);
    }

    [HttpGet]
    [Route("{projectId}/{id}/histogram")]
    public IActionResult GetHistogram(string projectId, string id)
    {
        var layer = GetLayer(projectId, id);
        var path = GetImagePath(layer);

        var img = Image.Load(path);


        return Ok();
    }


    [HttpPost]
    [Route("{projectId}/{id}/selection/fill")]
    public IActionResult FillLayerSelection(string projectId, string id, object color)
    {

        return Ok(color);
    }


    [HttpPost]
    [Route("upload/{projectId}")]
    public async Task<IActionResult> UploadLayer(string projectId, [FromForm] IFormFile image)
    {
        var project = _context.Projects.FirstOrDefault(p => p.Id == Guid.Parse(projectId));

        if (project == null)
        {
            return NotFound("Project not found.");
        }
        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
        var storedImageName = $"{Guid.NewGuid()}-{image.FileName}";

        var layer = new Layer(image.FileName, project.Id, baseUrl + "/images/" + storedImageName);
        layer.StroredImageName = storedImageName;
        layer.ProjectId = project.Id;

        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", storedImageName);

        _imageService.CreateFromFile(image, path);

        await _context.Layers.AddAsync(layer);
        await _context.SaveChangesAsync();


        return Ok();
    }

}

public class LayerFromSelectionRequest
{
    public required int[] Points { get; set; }
}

