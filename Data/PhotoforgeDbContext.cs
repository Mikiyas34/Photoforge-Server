using Microsoft.EntityFrameworkCore;
using Photoforge_Server.Models;

namespace Photoforge_Server.Data;

public class PhotoforgeDbContext : DbContext
{
    public PhotoforgeDbContext(DbContextOptions<PhotoforgeDbContext> options) : base(options) { }
    public DbSet<ProjectModel> Projects;
}
