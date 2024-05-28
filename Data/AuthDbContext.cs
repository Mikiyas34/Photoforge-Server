using Microsoft.EntityFrameworkCore;
using Photoforge_Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Photoforge_Server.Data;

public class AuthDbContext : IdentityDbContext<User>
{

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    override protected void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("users");

        builder.Entity<User>().HasMany(u => u.Projects).WithOne().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.NoAction);
    }

}
