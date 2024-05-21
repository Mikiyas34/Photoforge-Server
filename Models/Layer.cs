namespace Photoforge_Server.Models;

public class Layer
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; }
    public string StroredImageName { get; set; }
    public string Url { get; set; }
    public int X {  get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public float Brightness { get; set; }
    public float Contrast { get; set; }
    public float Saturation { get; set; }
    public float Lightness { get; set; }
    public float Hue { get; set; }


    public Layer(string Name,Guid ProjectId, string Url)
    {
        Id = Guid.NewGuid();
        this.Name = Name;
        this.Url = Url;
        this.ProjectId = ProjectId;
    }
}

