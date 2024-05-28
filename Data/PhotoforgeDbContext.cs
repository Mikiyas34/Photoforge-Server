using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Photoforge_Server.Models;

namespace Photoforge_Server.Data;

public class PhotoforgeDbContext : IdentityDbContext<User>
{
    public PhotoforgeDbContext(DbContextOptions<PhotoforgeDbContext> options) : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Layer> Layers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);
        builder.Entity<User>().HasMany(u => u.Projects).WithOne(p => p.User).HasForeignKey(p => p.UserId);
        builder.Entity<Layer>().HasOne(l => l.Project).WithMany(p => p.Layers).HasForeignKey(l => l.ProjectId);
    }


}
