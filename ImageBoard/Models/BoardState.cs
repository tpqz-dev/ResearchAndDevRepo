using System.Collections.Generic;

namespace ImageBoard.Models
{
    public class BoardState
    {
        public List<ImageItem> Images { get; set; } = new List<ImageItem>();
        public double WindowWidth { get; set; } = 1200;
        public double WindowHeight { get; set; } = 800;
        public double WindowTop { get; set; } = 100;
        public double WindowLeft { get; set; } = 100;
        public bool IsTopMost { get; set; }
        public double ViewTranslateX { get; set; }
        public double ViewTranslateY { get; set; }
        public double ViewScale { get; set; } = 1.0;
    }
}
