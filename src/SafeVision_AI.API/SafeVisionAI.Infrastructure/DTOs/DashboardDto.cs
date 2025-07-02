using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class DashboardDto
    {
        public DashboardStatsDto Stats { get; set; } = new();
        public List<IncidentDto> RecentIncidents { get; set; } = new();
        public List<CameraStatusDto> CameraStatuses { get; set; } = new();
        public List<AlertDto> RecentAlerts { get; set; } = new();
    }
}
