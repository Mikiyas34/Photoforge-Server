using Microsoft.AspNetCore.Identity;

namespace Photoforge_Server.Models;

public class User : IdentityUser
{

    public virtual ICollection<Project> Projects { get; }

}


