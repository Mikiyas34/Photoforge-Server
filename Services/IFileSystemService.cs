namespace Photoforge_Server.Services;

public interface IFileSystemService
{
    public void CreateFromFile(IFormFile file, string path);
}
