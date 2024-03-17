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
}