namespace Photoforge_Server.Models;
using System.ComponentModel.DataAnnotations;
public class Project
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifyedAt { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }


    public string UserId { get; set; }
    public virtual User User { get; set; } 

    public virtual List<Layer> Layers { get; }

    public Project(string name, string userId)
    {
        Id = Guid.NewGuid();
        Name = name;
        UserId = userId;
        CreatedAt = DateTime.Now;
        ModifyedAt = DateTime.Now;
    }

}