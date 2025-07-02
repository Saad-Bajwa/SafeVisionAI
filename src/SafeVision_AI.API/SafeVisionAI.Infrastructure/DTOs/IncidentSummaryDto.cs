using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class IncidentSummaryDto
    {
        public int TotalIncidents { get; set; }
        public int CriticalIncidents { get; set; }
        public int ResolvedIncidents { get; set; }
        public int PendingIncidents { get; set; }
        public List<IncidentTypeCountDto> IncidentsByType { get; set; } = new();
        public List<CameraIncidentCountDto> IncidentsByCamera { get; set; } = new();
    }
}
