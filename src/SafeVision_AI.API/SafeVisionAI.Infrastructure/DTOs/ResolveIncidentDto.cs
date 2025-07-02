using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class ResolveIncidentDto
    {
        public string ResolvedBy { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
