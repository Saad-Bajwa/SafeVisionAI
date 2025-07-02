using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class IncidentTypeCountDto
    {
        public string Type { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
