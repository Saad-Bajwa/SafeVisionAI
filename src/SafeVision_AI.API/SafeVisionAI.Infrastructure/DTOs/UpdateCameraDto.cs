using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class UpdateCameraDto
    {
        public string? Name { get; set; }
        public string? StreamUrl { get; set; }
        public string? Location { get; set; }
        public bool? IsActive { get; set; }
        public int? FrameRate { get; set; }
        public int? AnalysisInterval { get; set; }
    }
}
