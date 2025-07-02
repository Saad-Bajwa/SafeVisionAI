using System.ComponentModel.DataAnnotations;
using SafeVisionAI.Core.Enums;

namespace SafeVisionAI.Core.Entities
{
    public class ProcessingQueue
    {
        public int Id { get; set; }

        public int CameraId { get; set; }

        public ProcessingType Type { get; set; }

        public string? PayloadData { get; set; } 

        public ProcessingStatus Status { get; set; } = ProcessingStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }

        public int RetryCount { get; set; } = 0;
        public int MaxRetries { get; set; } = 3;

        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }

        // Navigation properties
        public virtual Camera Camera { get; set; } = null!;
    }
}
