using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class ReportAnalyticsDto
    {
        public List<HourlyIncidentDto> HourlyBreakdown { get; set; } = new();
        public List<IncidentTypeCountDto> TypeBreakdown { get; set; } = new();
        public List<CameraIncidentCountDto> CameraBreakdown { get; set; } = new();
        public ResponseTimeStatsDto ResponseTimes { get; set; } = new();
    }
}
