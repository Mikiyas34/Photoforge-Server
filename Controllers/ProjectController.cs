using Microsoft.AspNetCore.Mvc;
using Photoforge_Server.Data;
namespace Photoforge_Server.Controllers;

using Microsoft.Extensions.Logging;
using Photoforge_Server.Models;
using Photoforge_Server.Services;
using System.IO;

[ApiController]
[Route("projects/")]
    public class ProjectController : Controller
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
        var layer = new Layer(image.Name, project.Id, $"https://localhost:5274/images/{storedImageName}");

  
        await _context.Projects.AddAsync(project);
        await _context.Layers.AddAsync(layer);
        await _context.SaveChangesAsync();

        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        _fileSystemService.CreateFromFile(image, $"{path}/{storedImageName}");
        _logger.LogInformation("Image uploaded.");

        return Ok("Image uploaded.");
    }





    [HttpGet]
    [Route("{projectId}/layers")]
    public IActionResult GetLayersOfProject(string projectId)
    {
        var layers = _context.Layers.Where(layer => layer.ProjectId == Guid.Parse(projectId));
        return Ok(projectId);
    }


}


