using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class CameraStatusDto
    {
        public int CameraId { get; set; }
        public string CameraName { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsOnline { get; set; }
        public DateTime LastSeen { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
