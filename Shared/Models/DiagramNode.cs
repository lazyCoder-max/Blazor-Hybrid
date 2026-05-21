namespace Shared.Models
{
    public class DiagramNode
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = "";
        public string Type { get; set; } = "";

        public double X { get; set; }
        public double Y { get; set; }

        public double Width { get; set; } = 180;
        public double Height { get; set; } = 90;
    }
}
