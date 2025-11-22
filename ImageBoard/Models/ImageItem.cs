using System;

namespace ImageBoard.Models
{
    public class ImageItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string SourcePath { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Scale { get; set; } = 1.0;
        public bool IsFlippedX { get; set; }
        public bool IsFlippedY { get; set; }
        public int ZIndex { get; set; }
    }
}
