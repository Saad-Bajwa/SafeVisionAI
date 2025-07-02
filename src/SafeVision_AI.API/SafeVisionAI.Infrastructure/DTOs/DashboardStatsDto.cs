using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalCameras { get; set; }
        public int ActiveCameras { get; set; }
        public int TotalIncidentsToday { get; set; }
        public int CriticalIncidentsToday { get; set; }
        public int ResolvedIncidentsToday { get; set; }
        public double AverageResponseTime { get; set; } // in minutes
    }
}
