using System.ComponentModel.DataAnnotations;
using SafeVisionAI.Core.Enums;

namespace SafeVisionAI.Core.Entities
{
    public class IncidentAlert
    {
        public int Id { get; set; }

        public int IncidentId { get; set; }

        [Required]
        [MaxLength(200)]
        public string RecipientEmail { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? RecipientPhone { get; set; }

        public AlertStatus Status { get; set; } = AlertStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }
        public DateTime? AcknowledgedAt { get; set; }

        [MaxLength(500)]
        public string? ErrorMessage { get; set; }

        // Navigation properties
        public virtual Incident Incident { get; set; } = null!;
    }
}
