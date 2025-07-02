using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class DailyReportDto
    {
        public int Id { get; set; }
        public DateTime ReportDate { get; set; }
        public string Summary { get; set; } = string.Empty;
        public int TotalIncidents { get; set; }
        public int CriticalIncidents { get; set; }
        public int ResolvedIncidents { get; set; }
        public DateTime GeneratedAt { get; set; }
        public string? ReportFileUrl { get; set; }
        public ReportAnalyticsDto Analytics { get; set; } = new();
    }
}
