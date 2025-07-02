using System.ComponentModel.DataAnnotations;

namespace SafeVisionAI.Core.Entities
{
    public class Camera
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string StreamUrl { get; set; } = string.Empty;

        [MaxLength(255)]
        public string Location { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastSeen { get; set; } = DateTime.UtcNow;
        public int FrameRate { get; set; } = 30;
        public int AnalysisInterval { get; set; } = 3;
        public virtual ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }

}
