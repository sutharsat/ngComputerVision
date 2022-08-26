using System.Collections.Generic;

namespace ngComputerVision.Models
{
    public class AnalyzeResult
    {
        public List<ReadResult> readResults { get; set; }
        public string version { get; set; }
        public string modelVersion { get; set; }
    }
}
