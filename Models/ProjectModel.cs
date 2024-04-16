namespace Photoforge_Server.Models;
using System.ComponentModel.DataAnnotations;
public class ProjectModel
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifyedAt { get; set; }
    public int Width { get; set; }
    public int Height { get; set; } 


    public ProjectModel(string Name)
    {
        Id = Guid.NewGuid();
        this.Name = Name;
        CreatedAt = DateTime.Now;
        ModifyedAt = DateTime.Now;  
    }

}