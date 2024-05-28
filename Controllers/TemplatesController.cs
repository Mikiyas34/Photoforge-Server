using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Photoforge_Server.Data;

namespace Photoforge_Server.Controllers;

[Route("templates")]
[ApiController]
public class TemplatesController : ControllerBase
{
    private readonly PhotoforgeDbContext _context;
    public TemplatesController(PhotoforgeDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetTemplates()
    {
        return Ok();
    }
}
