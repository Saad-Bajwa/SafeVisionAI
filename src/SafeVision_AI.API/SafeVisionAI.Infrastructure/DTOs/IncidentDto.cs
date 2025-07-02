using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class IncidentDto
    {
        public int Id { get; set; }
        public int CameraId { get; set; }
        public string CameraName { get; set; } = string.Empty;
        public string CameraLocation { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double ConfidenceScore { get; set; }
        public string Severity { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
        public bool IsResolved { get; set; }
        public string? ResolvedBy { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? VideoClipUrl { get; set; }
        public string? ImageUrl { get; set; }
        public BoundingBoxDto? BoundingBox { get; set; }
        public string? Notes { get; set; }
    }
}
