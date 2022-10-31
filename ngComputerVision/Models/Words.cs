using System.Collections.Generic;

namespace ngComputerVision.Models
{
    public class Words
    {
        public List<int> boundingBox { get; set; }
        public string text { get; set; }
        public double confidence { get; set; }
    }
}
