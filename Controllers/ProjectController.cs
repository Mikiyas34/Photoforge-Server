using Microsoft.AspNetCore.Mvc;
using Photoforge_Server.Data;
namespace Photoforge_Server.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Photoforge_Server.Models;
using Photoforge_Server.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Security.Claims;

[Authorize]
[ApiController]
[Route("projects/")]
public partial class ProjectController : Controller
{
    private readonly PhotoforgeDbContext _context;
    private readonly IImageService _imageService;
    private readonly ILogger<ProjectController> _logger;
    private readonly UserManager<User> _userManager;
    public ProjectController(PhotoforgeDbContext context, IImageService imageService, UserManager<User> userManager, ILogger<ProjectController> logger)
    {
        _context = context;
        _imageService = imageService;
        _logger = logger;
        _userManager = userManager;
    }


    [HttpGet]
    public IActionResult GetProjects()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var projects = _context.Projects.Where(project => project.UserId == userId).ToList();
        return Ok(projects);
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile image)
    {

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User does not exist.");
        }

        if (!_imageService.IsImageSizeValid(image))
        {
            return BadRequest("The size of the image is too big.");
        }
        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";

        var storedImageName = $"{Guid.NewGuid()}-{image.FileName}";

        _logger.LogInformation("Adding project...");
        var project = new Project(image.FileName, user.Id);


        var layer = new Layer(image.FileName, project.Id, baseUrl + $"/images/{storedImageName}")
        {
            StroredImageName = storedImageName
        };

        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", storedImageName);

        _imageService.CreateFromFile(image, path);
        _logger.LogInformation("Image uploaded.");

        var imgInfo = Image.Identify(path);

        project.Width = imgInfo.Width;
        project.Height = imgInfo.Height;



        _logger.LogInformation("Saving changins...");
        await _context.Projects.AddAsync(project);
        await _context.Layers.AddAsync(layer);
        await _context.SaveChangesAsync();


        return Created();
    }



    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateProject(ProjectPreset preset)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return BadRequest("User does not exist.");
        }

        var storedImageName = $"{Guid.NewGuid()}.png";
        string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.PathBase}";
        var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", storedImageName);


        var image = _imageService.CreateImage(preset.Width, preset.Height, Color.White);
        image.SaveAsPng(path);

        var project = new Project(preset.Name, user.Id)
        {
            Width = image.Width,
            Height = image.Height
        };


        var layer = new Layer($"{preset.Name}.png", project.Id, baseUrl + "/images/" + storedImageName)
        {
            StroredImageName = storedImageName
        };

        await _context.Projects.AddAsync(project);
        await _context.Layers.AddAsync(layer);

        await _context.SaveChangesAsync();

        return Ok(project);
    }

    [HttpGet]
    [Route("{projectId}")]
    public IActionResult GetProject(string projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var project = _context.Projects.Where(p => p.Id == Guid.Parse(projectId) && p.UserId == userId).FirstOrDefault();
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var project = _context.Projects.FirstOrDefault(x => x.Id == Guid.Parse(projectId) && x.UserId == userId);

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


