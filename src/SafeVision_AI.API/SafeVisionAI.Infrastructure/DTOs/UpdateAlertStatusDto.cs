﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SafeVisionAI.Infrastructure.DTOs
{
    public class UpdateAlertStatusDto
    {
        public string Status { get; set; } = string.Empty; 
        public string? ErrorMessage { get; set; }
    }
}
