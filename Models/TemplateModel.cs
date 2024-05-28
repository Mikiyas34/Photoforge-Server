using Microsoft.EntityFrameworkCore;

namespace Photoforge_Server.Models;

public class TemplateModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Resolution { get; set; }
}
