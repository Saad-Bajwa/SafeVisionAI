using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SafeVisionAI.Core.Enums;

namespace SafeVisionAI.Core.Entities
{
    public class Incident
    {
        public int Id { get; set; }

        public int CameraId { get; set; }

        public IncidentType Type { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public double ConfidenceScore { get; set; }

        public IncidentSeverity Severity { get; set; }

        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;

        // File paths in Azure Blob Storage
        public string? VideoClipUrl { get; set; }
        public string? ImageUrl { get; set; }
        public string? AudioClipUrl { get; set; }

        // Bounding box for detected objects
        public int? BoundingBoxX { get; set; }
        public int? BoundingBoxY { get; set; }
        public int? BoundingBoxWidth { get; set; }
        public int? BoundingBoxHeight { get; set; }

        // Status tracking
        public bool IsResolved { get; set; } = false;
        public string? ResolvedBy { get; set; }
        public DateTime? ResolvedAt { get; set; }

        [MaxLength(2000)]
        public string? Notes { get; set; }

        // AI Processing metadata
        public string? ModelVersion { get; set; }
        public string? ProcessingDetails { get; set; } // JSON string

        // Navigation properties
        public virtual Camera Camera { get; set; } = null!;
        public virtual ICollection<IncidentAlert> Alerts { get; set; } = new List<IncidentAlert>();
    }
}
