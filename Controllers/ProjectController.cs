using Microsoft.AspNetCore.Mvc;
using Photoforge_Server.Data;
namespace Photoforge_Server.Controllers;

using Photoforge_Server.Models;
using Photoforge_Server.Services;
using System.Buffers;

[ApiController]
[Route("projects/")]
public class ProjectController : Controller
{
    private readonly PhotoforgeDbContext _context;
    private readonly IImageService _imageService;
    public ProjectController(PhotoforgeDbContext context, IImageService imageService)
    {
        _context = context;
        _imageService = imageService;
    }
    [HttpGet(Name = "GetProjects")]
    public object Get()
    {
        return Enumerable.Range(1, 5).Select(index => new ProjectModel
        {
            Id = Guid.NewGuid(),
            Name = "Blending images into Clouds.pfd " + index.ToString(),
            CreatedAt = DateTime.Now,
            ModifyedAt = DateTime.Now,
        })
        .ToArray();

    }

    [HttpGet]
    [Route("recent")]
    public object GetRecentProjects()
    {
        return Enumerable.Range(1, 5).Select(index => new ProjectModel
        {
            Id = Guid.NewGuid(),
            Name = "Blending image into Clouds.pfd" + index.ToString(),
            CreatedAt = DateTime.Now,
            ModifyedAt = DateTime.Now,
        }).ToArray();
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateProject(Project project)
    {
        // insert in the db
       var result = await _context.Projects.AddAsync(new ProjectModel {  Id = Guid.NewGuid(), CreatedAt = DateTime.Now, ModifyedAt = DateTime.Now, Name = project.Name});

        return Ok(project);
    }
}

public class Project
{
    public string Name { get; set; }
    public int Width { get; set; }

    public int Height { get; set; }

}
