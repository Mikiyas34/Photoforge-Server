using Microsoft.AspNetCore.Mvc;
using Photoforge_Server.Data;
namespace Photoforge_Server.Controllers;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Photoforge_Server.Models;
using Photoforge_Server.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;

[ApiController]
[Route("projects/")]
public partial class ProjectController : Controller
{
    private readonly PhotoforgeDbContext _context;
    private readonly IImageService _imageService;
    private readonly ILogger<ProjectController> _logger;
    private readonly IFileSystemService _fileSystemService;
    public ProjectController(PhotoforgeDbContext context, IImageService imageService, IFileSystemService fileSystemService, ILogger<ProjectController> logger)
    {
        _context = context;
        _imageService = imageService;
        _logger = logger;
        _fileSystemService = fileSystemService;
    }


    [HttpGet]
    public IActionResult GetProjects()
    {
        var projects = _context.Projects.ToList();
        return Ok(projects);
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile image)
    {
        _logger.LogInformation("Attempting to upload image...");

        var MAX_IMAGE_SIZE = 10_485_760; //10Mb

        if (image.Length >= MAX_IMAGE_SIZE)
        {
            _logger.LogError("The size of the image is too big.");
            _logger.LogInformation("Uploading canceld.");
            return BadRequest("The size of the image is too big.");
        }

        var storedImageName = $"{Guid.NewGuid()}-{image.FileName}";

        var project = new ProjectModel(image.FileName);
        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
        var layer = new Layer(image.FileName, project.Id, baseUrl+"/images/{storedImageName}");
        layer.StroredImageName = storedImageName;


        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", storedImageName);

        _fileSystemService.CreateFromFile(image, path);
        _logger.LogInformation("Image uploaded.");

        var imgInfo = Image.Identify(path);

        project.Width = imgInfo.Width;
        project.Height = imgInfo.Height;


        await _context.Projects.AddAsync(project);
        await _context.Layers.AddAsync(layer);
        await _context.SaveChangesAsync();


        return Ok("Image uploaded.");
    }



    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateProject(ProjectPreset preset)
    {
        
        var storedImageName = $"{Guid.NewGuid()}.png";
        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads",storedImageName );


        var image = _imageService.CreateImage(preset.Width, preset.Height, Color.White);
        image.SaveAsPng(path);

        var project = new ProjectModel(preset.Name);
        project.Width = image.Width;
        project.Height = image.Height;


        var layer = new Layer($"{preset.Name}.png", project.Id, baseUrl+"/images/"+storedImageName);
        layer.StroredImageName = storedImageName;

        await _context.Projects.AddAsync(project);
        await _context.Layers.AddAsync(layer);

        await _context.SaveChangesAsync();

        return Ok(project);
    }

    [HttpGet]
    [Route("{projectId}/layers")]
    public IActionResult GetLayersOfProject(string projectId)
    {
        var layers = _context.Layers.Where(layer => layer.ProjectId == Guid.Parse(projectId));
        return Ok(layers);
    }


    [HttpDelete]
    [Route("{projectId}")]
    public async Task<IActionResult> DeleteProject(string projectId)
    {
        var project = _context.Projects.FirstOrDefault(x => x.Id == Guid.Parse(projectId));

        if (project == null)
        {
            return NotFound();
        }

        var layers = _context.Layers.Where(layer => layer.ProjectId == Guid.Parse(projectId));

        await layers.ForEachAsync(layer =>
         {
             var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", layer.StroredImageName);
             _logger.LogInformation($"Attempting to remove file {imagePath}");
             if (System.IO.File.Exists(imagePath))
             {
                 System.IO.File.Delete(imagePath);
             }
         });

        _context.Layers.RemoveRange(layers);
        _context.Projects.Remove(project);
        _context.SaveChanges();

        return NoContent();
    }



}


