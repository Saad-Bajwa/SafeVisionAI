using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class CameraIncidentCountDto
    {
        public int CameraId { get; set; }
        public string CameraName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
