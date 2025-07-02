using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Core.Entities
{
    public class DailyReport
    {
        public int Id { get; set; }

        public DateTime ReportDate { get; set; }

        public string Summary { get; set; } = string.Empty;

        public int TotalIncidents { get; set; }
        public int CriticalIncidents { get; set; }
        public int HighIncidents { get; set; }
        public int MediumIncidents { get; set; }
        public int LowIncidents { get; set; }
        public int ResolvedIncidents { get; set; }

        public string? GeneratedBy { get; set; }
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

        // JSON data for charts and detailed analytics
        public string ReportData { get; set; } = "{}";

        // Report file URL in Azure Blob Storage
        public string? ReportFileUrl { get; set; }
    }
}
