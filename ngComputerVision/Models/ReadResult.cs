using System.Collections.Generic;

namespace ngComputerVision.Models
{
    public class ReadResult
    {

        public int page { get; set; }
        public int angle { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string unit { get; set; }
        public string language { get; set; }
        public List<Lines> lines { get; set; }
    }
}
