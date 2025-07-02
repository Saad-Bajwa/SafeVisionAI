using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class CreateCameraDto
    {
        public string Name { get; set; } = string.Empty;
        public string StreamUrl { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int FrameRate { get; set; } = 30;
        public int AnalysisInterval { get; set; } = 3;
    }
}
