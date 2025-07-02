using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class ResponseTimeStatsDto
    {
        public double AverageMinutes { get; set; }
        public double MedianMinutes { get; set; }
        public double FastestMinutes { get; set; }
        public double SlowestMinutes { get; set; }
    }
}
