namespace Photoforge_Server
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ModifyedAt { get; set;}
        public int Width { get; set; }
        public int Height { get; set; }

    }
}
