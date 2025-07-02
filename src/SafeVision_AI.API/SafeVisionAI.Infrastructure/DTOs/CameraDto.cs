using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class CameraDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StreamUrl { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime LastSeen { get; set; }
        public int TotalIncidents { get; set; }
        public int TodayIncidents { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
