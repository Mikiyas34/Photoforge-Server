
namespace Photoforge_Server.Services;

public class FileSystemService : IFileSystemService
{

    public void CreateFromFile(IFormFile file, string path)
    {
            using (var newfile = File.Create(path))
            {
                file.CopyTo(newfile);
            
            }
    }
}
