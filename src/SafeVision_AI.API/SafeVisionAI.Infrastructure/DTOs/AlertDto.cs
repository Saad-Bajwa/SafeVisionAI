using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class AlertDto
    {
        public int Id { get; set; }
        public int IncidentId { get; set; }
        public string IncidentType { get; set; } = string.Empty;
        public string CameraName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string RecipientEmail { get; set; } = string.Empty;
        public string RecipientPhone { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? AcknowledgedAt { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
