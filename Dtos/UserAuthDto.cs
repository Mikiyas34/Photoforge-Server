using System.ComponentModel.DataAnnotations;

namespace Photoforge_Server.Dtos;

public class UserAuthDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}