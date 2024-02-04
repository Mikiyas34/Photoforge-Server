using Microsoft.AspNetCore.Mvc;

namespace Photoforge_Server.Controllers;

    [ApiController]
    [Route("projects/")]
    public class ProjectController : Controller
    {

    [HttpGet(Name = "GetProjects")]
    public IEnumerable<Project> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Project
        {
            Id = Guid.NewGuid(),
            Name = "project " + index.ToString(),
            CreatedAt = DateTime.Now,
            ModifyedAt = DateTime.Now,
            Width = 100 + index,
            Height = 100 + index,
        })
        .ToArray();
    }
}

