﻿namespace Photoforge_Server.Services;

class PixelLayer
{
    public Guid Id { get; set; }
    public string ProjectId { get; set; }
    public IFormFile Image { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public Filter[] filters { get; set; }
}


